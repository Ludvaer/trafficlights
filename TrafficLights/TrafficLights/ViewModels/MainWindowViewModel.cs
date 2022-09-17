using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TrafficLights.Enums;
using TrafficLights.Models;

namespace TrafficLights.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Цвет при выключенном сигнале
        /// </summary>
        private readonly IBrush OffColor = Brushes.Black;

        /// <summary>
        /// Нажатие на красную кнопку
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressRedCommand { get; }

        /// <summary>
        /// Нажатие на жёлтую кнопку
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressYellowCommand { get; }

        /// <summary>
        /// Нажатие на зелёную кнопку
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressGreenCommand { get; }

        /// <summary>
        /// Кнопка проверки огней
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressCheckCommand { get; }

        /// <summary>
        /// Кнопка мигания жёлтого
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressBlinkCommand { get; }

        /// <summary>
        /// Кнопка следующего шага автомата
        /// </summary>
        public ReactiveCommand<Unit,Unit> PressNextStepCommand { get; }

        /// <summary>
        /// Цвет красного огня
        /// </summary>
        private IBrush _redColor;

        /// <summary>
        /// Код для доступа к красному огню светофора
        /// </summary>
        public IBrush RedColor
        {
            get => _redColor;
            set => this.RaiseAndSetIfChanged(ref _redColor, value);
        }

        /// <summary>
        /// Цвет жёлтого огня
        /// </summary>
        private IBrush _yellowColor;

        /// <summary>
        /// Код для доступа к жёлтому огню светофора
        /// </summary>
        public IBrush YellowColor
        {
            get => _yellowColor;
            set => this.RaiseAndSetIfChanged(ref _yellowColor, value);
        }

        /// <summary>
        /// Цвет зелёного огня
        /// </summary>
        private IBrush _greenColor;

        /// <summary>
        /// Код для доступа к зелёному огню светофора
        /// </summary>
        public IBrush GreenColor
        {
            get => _greenColor;
            set => this.RaiseAndSetIfChanged(ref _greenColor, value);
        }

        /// <summary>
        /// Текст в консоли
        /// </summary>
        private string _consoleText;

        /// <summary>
        /// Код для доступа к тексту в консоли
        /// </summary>
        public string ConsoleText
        {
            get => _consoleText;
            set => this.RaiseAndSetIfChanged(ref _consoleText, value);
        }

        /// <summary>
        /// Модель светофора
        /// </summary>
        private TrafficLightsModel _model;

        /// <summary>
        /// Таймер для проверки ламп
        /// </summary>
        private System.Timers.Timer _checkLampsTimer;

        /// <summary>
        /// Таймер для мигания
        /// </summary>
        private System.Timers.Timer _blinkTimer;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel(TrafficLightsModel trafficLightsModel)
        {
            _model = trafficLightsModel;

            PressRedCommand = ReactiveCommand.Create(OnRedPressed); // Связывание метода с командой
            PressYellowCommand = ReactiveCommand.Create(OnYellowPressed);
            PressGreenCommand = ReactiveCommand.Create(OnGreenPressed);
            PressCheckCommand = ReactiveCommand.Create(OnCheckPressed);
            PressBlinkCommand = ReactiveCommand.Create(OnBlinkPressed);
            PressNextStepCommand = ReactiveCommand.Create(OnAutomatStep);

            ProcessState();

            // Настройка таймера мигания
            _blinkTimer = new System.Timers.Timer(TrafficLightsModel.BlinkSpeed);
            _blinkTimer.AutoReset = true;
            _blinkTimer.Enabled = true;

            _blinkTimer.Elapsed += OnBlinkTimeoutEvent;
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата красная кнопка
        /// </summary>
        private void OnRedPressed()
        {
            _model.IsRedLightOn = !_model.IsRedLightOn; //! - меняет значение булевой переменной на противоположное

            ProcessState();

            AddLineToConsole("Нажата красная кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата жёлтая кнопка
        /// </summary>
        private void OnYellowPressed()
        {
            _model.IsYellowLightOn = !_model.IsYellowLightOn;

            ProcessState();

            AddLineToConsole("Нажата жёлтая кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата зелёная кнопка
        /// </summary>
        private void OnGreenPressed()
        {
            _model.IsGreenLightOn = !_model.IsGreenLightOn;

            ProcessState();

            AddLineToConsole("Нажата зелёная кнопка");
        }

        /// <summary>
        /// Метод, вызываемый при нажатиии кнопки проверки
        /// </summary>
        private void OnCheckPressed()
        {
            _model.IsRedLightOn = true;
            _model.IsYellowLightOn = true;
            _model.IsGreenLightOn = true;

            ProcessState();

            // Настройка таймера
            _checkLampsTimer = new System.Timers.Timer(TrafficLightsModel.CheckLength);
            _checkLampsTimer.AutoReset = false;
            _checkLampsTimer.Enabled = true;

            _checkLampsTimer.Elapsed += OnCheckTimeoutEvent;

            AddLineToConsole("Зажигаем все лампы");
        }

        /// <summary>
        /// Метод, вызываемый при нажатии кнопки мигания
        /// </summary>
        private void OnBlinkPressed()
        {
            _model.RedLightState = LightStateEnum.Blinking;
            _model.YellowLightState = LightStateEnum.Blinking;
            _model.GreenLightState = LightStateEnum.Blinking;
        }

        /// <summary>
        /// Обработчик состояний (в частности - цвет сигнала)
        /// </summary>
        private void ProcessState()
        {
            if (_model.RedLightState == LightStateEnum.On)
            {
                _model.IsRedLightOn = true;
            }
            else if (_model.RedLightState == LightStateEnum.Off)
            {
                _model.IsRedLightOn = false;
            }

            if (_model.YellowLightState == LightStateEnum.On)
            {
                _model.IsYellowLightOn = true;
            }
            else if (_model.YellowLightState == LightStateEnum.Off)
            {
                _model.IsYellowLightOn = false;
            }

            if (_model.GreenLightState == LightStateEnum.On)
            {
                _model.IsGreenLightOn = true;
            }
            else if (_model.GreenLightState == LightStateEnum.Off)
            {
                _model.IsGreenLightOn = false;
            }

            RedColor = _model.IsRedLightOn ? Brushes.Red : OffColor;
            YellowColor = _model.IsYellowLightOn ? Brushes.Yellow : OffColor;
            GreenColor = _model.IsGreenLightOn ? Brushes.Green : OffColor;
        }

        /// <summary>
        /// Метод, вызываемый
        /// </summary>
        /// <param name="source">Таймер, который вызвал метод</param>
        /// <param name="e">Параметры истечения времени</param>
        private void OnCheckTimeoutEvent(Object source, ElapsedEventArgs e)
        {
            _model.IsRedLightOn = false;
            _model.IsYellowLightOn = false;
            _model.IsGreenLightOn = false;

            ProcessState();

            AddLineToConsole("Проверка завершена, гасим все лампы");
        }

        /// <summary>
        /// Обработчик таймера мигания
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnBlinkTimeoutEvent(Object source, ElapsedEventArgs e)
        {
            if (_model.RedLightState == LightStateEnum.Blinking)
            {
                _model.IsRedLightOn = !_model.IsRedLightOn;
            }

            if (_model.YellowLightState == LightStateEnum.Blinking)
            {
                _model.IsYellowLightOn = !_model.IsYellowLightOn;
            }

            if (_model.GreenLightState == LightStateEnum.Blinking)
            {
                _model.IsGreenLightOn = !_model.IsGreenLightOn;
            }

            ProcessState();
        }

        /// <summary>
        /// Добавляет новую строку в консоль
        /// </summary>
        public void AddLineToConsole(string line)
        {
            ConsoleText += $"{line}{Environment.NewLine}";
        }

        #region Автомат


        /// <summary>
        /// Шаг автомата
        /// </summary>
        private void OnAutomatStep()
        {
            switch(_model.CurrentState)
            {
                // Сейчас светофор горит зелёным
                case TrafficLightState.Green:

                    // Горел зелёным, становится жёлтым
                    _model.CurrentState = TrafficLightState.YellowToRed;

                    _model.GreenLightState = LightStateEnum.Off;
                    _model.YellowLightState = LightStateEnum.On;

                    break;

                // Сейчас светофор горит жёлтым, следующий - красный
                case TrafficLightState.YellowToRed:

                    // Горел жёлтым, становится красным
                    _model.CurrentState = TrafficLightState.Red;

                    _model.YellowLightState = LightStateEnum.Off;
                    _model.RedLightState = LightStateEnum.On;

                    break;

                // Сейчас светофор горит красным
                case TrafficLightState.Red:

                    // Горел красным, становится жёлтым
                    _model.CurrentState = TrafficLightState.YellowToGreen;

                    _model.RedLightState = LightStateEnum.Off;
                    _model.YellowLightState = LightStateEnum.On;

                    break;

                // Сейчас светофор горит жёлтым, следующий - зелёный
                case TrafficLightState.YellowToGreen:

                    // Горел жёлтым, становится зелёным
                    _model.CurrentState = TrafficLightState.Green;

                    _model.YellowLightState = LightStateEnum.Off;
                    _model.GreenLightState = LightStateEnum.On;

                    break;

                default:
                    throw new InvalidOperationException("Некорректное текущее состояние автомата");
            }

            ProcessState();
        }


        #endregion
    }
}

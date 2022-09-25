using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TrafficLights.Enums;
using TrafficLights.Interfaces;
using TrafficLights.Models;

namespace TrafficLights.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
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
        /// Таймер для автомата
        /// </summary>
        private System.Timers.Timer _automatTimer;

        /// <summary>
        /// Интерфейс для управления огнями реального светофора
        /// </summary>
        private readonly ITrafficLights _trafficLights;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel(TrafficLightsModel trafficLightsModel)
        {
            _model = trafficLightsModel;
            
            // Запоминаем конкретный светофор
            _trafficLights = Program.Di.GetService<ITrafficLights>();
            _trafficLights.Setup(_model, this);


            PressRedCommand = ReactiveCommand.Create(OnRedPressed); // Связывание метода с командой
            PressYellowCommand = ReactiveCommand.Create(OnYellowPressed);
            PressGreenCommand = ReactiveCommand.Create(OnGreenPressed);
            PressCheckCommand = ReactiveCommand.Create(OnCheckPressed);
            PressBlinkCommand = ReactiveCommand.Create(OnBlinkPressed);

            // Найстройка таймера автомата
            _automatTimer = new System.Timers.Timer(TrafficLightsModel.GreenDuration);
            _automatTimer.AutoReset = true;
            _automatTimer.Enabled = true;

            _automatTimer.Elapsed += OnAutomatStep;
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата красная кнопка
        /// </summary>
        private void OnRedPressed()
        {
            _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.Blinking);

            AddLineToConsole("Нажата красная кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата жёлтая кнопка
        /// </summary>
        private void OnYellowPressed()
        {
            _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.Blinking);

            AddLineToConsole("Нажата жёлтая кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата зелёная кнопка
        /// </summary>
        private void OnGreenPressed()
        {
            _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Blinking);

            AddLineToConsole("Нажата зелёная кнопка");
        }

        /// <summary>
        /// Метод, вызываемый при нажатиии кнопки проверки
        /// </summary>
        private void OnCheckPressed()
        {
            _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.On);
            _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.On);
            _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.On);

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
            _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.Blinking);
            _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.Blinking);
            _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Blinking);
        }

        /// <summary>
        /// Метод, вызываемый
        /// </summary>
        /// <param name="source">Таймер, который вызвал метод</param>
        /// <param name="e">Параметры истечения времени</param>
        private void OnCheckTimeoutEvent(Object source, ElapsedEventArgs e)
        {
            _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Off);
            _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.Off);
            _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.Off);

            AddLineToConsole("Проверка завершена, гасим все лампы");
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
        private void OnAutomatStep(Object source, ElapsedEventArgs e)
        {
            switch(_model.CurrentState)
            {
                // Горит зелёным
                case TrafficLightState.Green:

                    _model.CurrentState = TrafficLightState.BlinkingGreen;

                    _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Blinking);
                    _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.Off);
                    _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.Off);

                    SetTimerInterval(TrafficLightsModel.BlinkingGreenDuration);
                    break;

                // Мигает зелёным
                case TrafficLightState.BlinkingGreen:

                    _model.CurrentState = TrafficLightState.Yellow;

                    _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Off);
                    _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.On);
                    _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.Off);

                    SetTimerInterval(TrafficLightsModel.YellowDuration);
                    break;

                // Горит жёлтый
                case TrafficLightState.Yellow:

                    _model.CurrentState = TrafficLightState.Red;

                    _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Off);
                    _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.Off);
                    _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.On);

                    SetTimerInterval(TrafficLightsModel.RedDuration);
                    break;

                // Горит красный
                case TrafficLightState.Red:

                    _model.CurrentState = TrafficLightState.RedAndYellow;

                    _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.Off);
                    _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.On);
                    _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.On);

                    SetTimerInterval(TrafficLightsModel.RedAndYellowDuration);
                    break;

                // Горит красный и жёлтый
                case TrafficLightState.RedAndYellow:

                    _model.CurrentState = TrafficLightState.Green;

                    _trafficLights.ChangeLightState(LightEnum.Green, LightStateEnum.On);
                    _trafficLights.ChangeLightState(LightEnum.Yellow, LightStateEnum.Off);
                    _trafficLights.ChangeLightState(LightEnum.Red, LightStateEnum.Off);

                    SetTimerInterval(TrafficLightsModel.GreenDuration);
                    break;

                default:
                    throw new InvalidOperationException("Некорректное текущее состояние автомата");
            }
        }

        /// <summary>
        /// Настраиваем интервал таймера
        /// </summary>
        private void SetTimerInterval(int interval)
        {
            _automatTimer.Stop();
            _automatTimer.Interval = interval;
            _automatTimer.Start();
        }



        #endregion
    }
}

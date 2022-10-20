using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TrafficLights.Models;

namespace TrafficLights.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Properties

        /// <summary>
        /// Цвет при выключенном сигнале
        /// </summary>
        private readonly IBrush OffColor = Brushes.Black;


        /// <summary>
        /// Кнопка проверки огней
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressCheckCommand { get; }

        /// <summary>
        /// Нажатие на красную кнопку
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressRedCommand => RedTrafficLightColor.PressCommand;

        /// <summary>
        /// Нажатие на жёлтую кнопку
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressYellowCommand => YellowTrafficLightColor.PressCommand;

        /// <summary>
        /// Нажатие на зелёную кнопку
        /// </summary>
        public ReactiveCommand<Unit, Unit> PressGreenCommand => GreenTrafficLightColor.PressCommand;

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
        /// Таймер для мигания
        /// </summary>
        private System.Timers.Timer _blinkTimer;


        #endregion



        /// <summary>
        /// Таймер для проверки ламп
        /// </summary>
        private System.Timers.Timer _checkLampsTimer;


        /// <summary>
        /// Обработчик состояний (в частности - цвет сигнала)
        /// </summary>
        private void ProcessState()
        {
            foreach(var color in Colors)
            {
                color.ProcessState();
            }
        }




        private TrafficLightsModel _model;


        /// <summary>
        /// Класс для избегания копипасты кода для каждого из трёх цветов.
        /// Объект поддерживает логику одногоиз цветов внутри MainWindowViewModel
        /// </summary>
        public abstract class TrafficLightColorViewModel : ViewModelBase
        {
            /// <summary>
            /// модель основного окна для обращения кобщим полям иметодам
            /// </summary>
            protected MainWindowViewModel _mainViewModel;
            /// <summary>
            /// имя цвета
            /// </summary>
            protected string _colorName;
            /// <summary>
            /// имя цвета в виде прилагательного для использования со словом кнопка
            /// </summary>
            protected string _colorNameButtonAdjective;
            /// <summary>
            /// Цвет которым рисовать включённый свет
            /// </summary>
            protected IBrush _onColor;
            /// <summary>
            /// проперти которое надо переопредлить так чтобы оно синхронизировалось с моделью
            /// </summary>
            public abstract bool ModelIsLightOn { get; set; }

       
            public TrafficLightColorViewModel(MainWindowViewModel mainViewModel)
            {
                _mainViewModel = mainViewModel;
                PressCommand = ReactiveCommand.Create(OnPressed);
            }
            
            /// <summary>
            /// команда нажатия на кнопку включения света
            /// </summary>
            public ReactiveCommand<Unit, Unit> PressCommand { get; }

            /// <summary>
            ///  Свойство которое должно быть связано с цветом которым сейчас отрисована лампа световора в интерфейсе
            /// </summary>
            public virtual IBrush LightColor
            {
                get; set;
            }

   
            /// <summary>
            /// метод обработки нажатия кнопки включения соответствующего цвета
            /// </summary>
            public void Toggle()
            {
                ModelIsLightOn = !ModelIsLightOn;
                /*_mainViewModel.*/ProcessState();
            }
            private void OnPressed()
            {
                Toggle();
                _mainViewModel.AddLineToConsole($"Нажата {_colorNameButtonAdjective} кнопка");
            }

                /// <summary>
                /// метод проверки состояния и отрисовки лампы
                /// </summary>
                public void ProcessState()
            {
                LightColor = ModelIsLightOn ? _onColor : _mainViewModel.OffColor;
            }
        }
        /// <summary>
        /// реализация логики красного цвета
        /// </summary>
        public class TrafficLightColorViewModelRed : TrafficLightColorViewModel
        {
            public TrafficLightColorViewModelRed(MainWindowViewModel mainViewModel): base(mainViewModel)
            {
                _colorName = "красный";
                _colorNameButtonAdjective = "красная";
                _onColor = Brushes.Red;
            }
            public override IBrush LightColor { get => _mainViewModel.RedColor; set => _mainViewModel.RedColor = value; }
            public override bool ModelIsLightOn { get => _mainViewModel._model.IsRedLightOn; set => _mainViewModel._model.IsRedLightOn = value; }
        }
        /// <summary>
        /// реализация логики жёлтого цвета
        /// </summary>
        public class TrafficLightColorViewModelYellow : TrafficLightColorViewModel
        {
            public TrafficLightColorViewModelYellow(MainWindowViewModel mainViewModel) : base(mainViewModel)
            {
                _colorName = "жёлтый";
                _colorNameButtonAdjective = "жёлтая";
                _onColor = Brushes.Yellow;
            }
            public override IBrush LightColor { get => _mainViewModel.YellowColor; set => _mainViewModel.YellowColor = value; }
            public override bool ModelIsLightOn { get => _mainViewModel._model.IsYellowLightOn; set => _mainViewModel._model.IsYellowLightOn = value; }
        }

        /// <summary>
        /// реализация логики зелёного цвета
        /// </summary>
        public class TrafficLightColorViewModelGreen : TrafficLightColorViewModel
        {
            public TrafficLightColorViewModelGreen(MainWindowViewModel mainViewModel) : base(mainViewModel)
            {
                _colorName = "зелёный";
                _colorNameButtonAdjective = "зелёная";
                _onColor = Brushes.MediumSeaGreen; //[Medium]Turquoise may be fine too
            }
            public override IBrush LightColor { get => _mainViewModel.GreenColor; set => _mainViewModel.GreenColor = value; }
            public override bool ModelIsLightOn { get => _mainViewModel._model.IsGreenLightOn; set => _mainViewModel._model.IsGreenLightOn = value; }
        }


        public TrafficLightColorViewModelRed RedTrafficLightColor;
        public TrafficLightColorViewModelYellow YellowTrafficLightColor;
        public TrafficLightColorViewModelGreen GreenTrafficLightColor;

        private TrafficLightColorViewModel[] Colors;

        public MainWindowViewModel(TrafficLightsModel model)
        {
            _model = model;
            PressCheckCommand = ReactiveCommand.Create(OnCheckPressed);
            Colors = new TrafficLightColorViewModel[]
            {
                RedTrafficLightColor = new TrafficLightColorViewModelRed(this),
                YellowTrafficLightColor = new TrafficLightColorViewModelYellow(this),
                GreenTrafficLightColor = new TrafficLightColorViewModelGreen(this),
            };
            PressBlinkCommand = ReactiveCommand.Create(OnBlinkPressed);

            ProcessState();

            // Настройка таймера мигания
            _blinkTimer = new System.Timers.Timer(TrafficLightsModel.BlinkSpeed);
            _blinkTimer.AutoReset = true;

            _blinkTimer.Elapsed += OnBlinkTimeoutEvent;
        }

        /// <summary>
        /// Метод, вызываемый при нажатии кнопки мигания
        /// </summary>
        private void OnBlinkPressed()
        {
            _blinkTimer.Start();
        }

        /// <summary>
        /// Обработчик таймера мигания
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnBlinkTimeoutEvent(Object source, ElapsedEventArgs e)
        {
            YellowTrafficLightColor.Toggle();
        }

        /// <summary>
        /// Метод, вызываемый при нажатиии кнопки проверки
        /// </summary>
        private void OnCheckPressed()
        {
            foreach (var color in Colors)
                color.ModelIsLightOn = true;

            ProcessState();

            _checkLampsTimer = new System.Timers.Timer(TrafficLightsModel.CheckLength);
            _checkLampsTimer.AutoReset = false;
            _checkLampsTimer.Enabled = true;

            _checkLampsTimer.Elapsed += OnCheckTimeoutEvent;

            AddLineToConsole("Зажигаем все лампы");
        }

        /// <summary>
        /// Метод, вызываемый при истичении таймера проверки
        /// </summary>
        /// <param name="source">Таймер, который вызвал метод</param>
        /// <param name="e">Параметры истечения времени</param>
        private void OnCheckTimeoutEvent(Object source, ElapsedEventArgs e)
        {
            foreach (var color in Colors)
                color.ModelIsLightOn = false;

            ProcessState();

            AddLineToConsole("Проверка завершена, гасим все лампы");
        }

        /// <summary>
        /// Добавляет новую строку в консоль
        /// </summary>
        public void AddLineToConsole(string line)
        {
            ConsoleText += $"{line}{Environment.NewLine}";
        }
    }
}

using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
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
        /// Конструктор
        /// </summary>
        public MainWindowViewModel(TrafficLightsModel trafficLightsModel)
        {
            _model = trafficLightsModel;

            PressRedCommand = ReactiveCommand.Create(OnRedPressed); // Связывание метода с командой
            PressYellowCommand = ReactiveCommand.Create(OnYellowPressed);
            PressGreenCommand = ReactiveCommand.Create(OnGreenPressed);

            RedColor = OffColor;
            YellowColor = OffColor;
            GreenColor = OffColor;
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата красная кнопка
        /// </summary>
        private void OnRedPressed()
        {
            _model.IsRedLightOn = !_model.IsRedLightOn; //! - меняет значение булевой переменной на противоположное

            RedColor = _model.IsRedLightOn ? Brushes.Red : OffColor;

            AddLineToConsole("Нажата красная кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата жёлтая кнопка
        /// </summary>
        private void OnYellowPressed()
        {
            _model.IsYellowLightOn = !_model.IsYellowLightOn;

            YellowColor = _model.IsYellowLightOn ? Brushes.Yellow : OffColor;

            AddLineToConsole("Нажата жёлтая кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата зелёная кнопка
        /// </summary>
        private void OnGreenPressed()
        {
            _model.IsGreenLightOn = !_model.IsGreenLightOn;

            GreenColor = _model.IsGreenLightOn ? Brushes.Green : OffColor;

            AddLineToConsole("Нажата зелёная кнопка");
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

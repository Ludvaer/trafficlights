using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

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
        /// Цвет красной лампы
        /// </summary>
        private IBrush _redColor;
        /// <summary>
        /// Цвет жёлтой лампы
        /// </summary>
        private IBrush _yellowColor;
        /// <summary>
        /// Цвет зелёной лампы
        /// </summary>
        private IBrush _greenColor;

        /// <summary>
        /// ПЦвет красной лампы светофора
        /// </summary>
        public IBrush RedLightColor
        {
            get => _redColor;
            set => this.RaiseAndSetIfChanged(ref _redColor, value);
        }
        /// <summary>
        /// ПЦвет жёлтой лампы светофора
        /// </summary>
        public IBrush YellowLightColor
        {
            get => _yellowColor;
            set => this.RaiseAndSetIfChanged(ref _yellowColor, value);
        }
        /// <summary>
        /// ПЦвет зелёной лампы светофора
        /// </summary>
        public IBrush GreenLightColor
        {
            get => _greenColor;
            set => this.RaiseAndSetIfChanged(ref _greenColor, value);
        }

        /// <summary>
        /// Установка выбранного цвета. Выключение ненужных, включение нужнго света.
        /// </summary>
        /// <param name="color"></param>
        private void  SetColor(TrafficLightColor color)
        {
            
            _currentLight = color;
            RedLightColor = color == TrafficLightColor.Red ? Brushes.Red : Brushes.Black;
            YellowLightColor = color == TrafficLightColor.Yellow ? Brushes.Yellow : Brushes.Black;
            GreenLightColor = color == TrafficLightColor.Green ? Brushes.Green : Brushes.Black;

        }


        /// <summary>
        /// Устанавливает цве, но отключает его если передать текущий.
        /// </summary>
        /// <param name="color"></param>
        private void SwitchColor(TrafficLightColor color)
        {
            if (_currentLight == color)
                SetColor(TrafficLightColor.Black);
            else
                SetColor(color);
        }

        /// <summary>
        /// Возможные цвета светофора.
        /// </summary>
        enum TrafficLightColor
        {
            Black, //turned off
            Red,
            Yellow,
            Green,          
        }

        private TrafficLightColor _currentLight;
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

        public MainWindowViewModel()
        {
            PressRedCommand = ReactiveCommand.Create(OnRedPressed); // Связывание метода с командой
            PressYellowCommand = ReactiveCommand.Create(OnYellowPressed);
            PressGreenCommand = ReactiveCommand.Create(OnGreenPressed);
            SetColor(TrafficLightColor.Black);
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата красная кнопка
        /// </summary>
        private void OnRedPressed()
        {
            SwitchColor(TrafficLightColor.Red);
            AddLineToConsole("Нажата красная кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата жёлтая кнопка
        /// </summary>
        private void OnYellowPressed()
        {
            SwitchColor(TrafficLightColor.Yellow);
            AddLineToConsole("Нажата жёлтая кнопка");
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата зелёная кнопка
        /// </summary>
        private void OnGreenPressed()
        {
            SwitchColor(TrafficLightColor.Green);
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

﻿using Avalonia.Media;
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

        #region Properties
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
        /// Цвет зелёной лампы светофора
        /// </summary>
        public IBrush GreenLightColor
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


        #endregion


        /// <summary>
        /// Возможные цвета светофора.
        /// </summary>
        enum TrafficLightColor
        {
            Black,
            Red,
            Yellow,
            Green,    
        }


        /// <summary>
        /// Установка выбранного цвета. Выключение ненужных, включение нужнго света.
        /// </summary>
        /// <param name="color"></param>
        private void  SetColor(TrafficLightColor color)
        {
            _model.IsRedLightOn = color == TrafficLightColor.Red;
            _model.IsYellowLightOn = color == TrafficLightColor.Yellow;
            _model.IsGreenLightOn = color == TrafficLightColor.Green;
            ProcessState();

        }
        /// <summary>
        /// Обработчик состояний (в частности - цвет сигнала)
        /// </summary>
        private void ProcessState()
        {
            RedLightColor = _model.IsRedLightOn ? Brushes.Red : OffColor;
            YellowLightColor = _model.IsYellowLightOn ? Brushes.Yellow : OffColor;
            GreenLightColor = _model.IsGreenLightOn ? Brushes.MediumSeaGreen : OffColor; //[Medium]Turquoise may be fine too
        }

        /// <summary>
        /// проверяет ключённость света
        /// </summary>
        /// <param name="color"></param>
        private bool IsLightOn(TrafficLightColor color)
        {
            switch (color)
            {
                case TrafficLightColor.Red:
                    return _model.IsRedLightOn;
                case TrafficLightColor.Yellow:
                    return _model.IsYellowLightOn;
                case TrafficLightColor.Green:
                    return _model.IsGreenLightOn;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Общий метод нажатия кнопки.
        /// </summary>
        /// <param name="color"></param>
        private void ToggleColoredButton(TrafficLightColor color)
        {
            if (IsLightOn(color))
                SetColor(TrafficLightColor.Black);
            else
                SetColor(color);
        }

        private TrafficLightsModel _model;



        public MainWindowViewModel(TrafficLightsModel model)
        {
            _model = model;
            PressRedCommand = ReactiveCommand.Create(OnRedPressed); // Связывание метода с командой
            PressYellowCommand = ReactiveCommand.Create(OnYellowPressed);
            PressGreenCommand = ReactiveCommand.Create(OnGreenPressed);
            PressCheckCommand = ReactiveCommand.Create(OnCheckPressed);
            SetColor(TrafficLightColor.Black);
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата красная кнопка
        /// </summary>
        private void OnRedPressed()
        {
            ToggleColoredButton(TrafficLightColor.Red);
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата жёлтая кнопка
        /// </summary>
        private void OnYellowPressed()
        {
            ToggleColoredButton(TrafficLightColor.Yellow);
        }

        /// <summary>
        /// Метод, вызываемый, когда нажата зелёная кнопка
        /// </summary>
        private void OnGreenPressed()
        {
            ToggleColoredButton(TrafficLightColor.Green);
          
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

            AddLineToConsole("Зажигаем все лампы");
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

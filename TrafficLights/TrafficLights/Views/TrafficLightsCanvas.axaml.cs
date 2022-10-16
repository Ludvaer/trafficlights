using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Windows.Input;

namespace TrafficLights.Views
{
    public partial class TrafficLightsCanvas : UserControl
    {
        /// <summary>
        /// Цвет корпуса
        /// </summary>
        private readonly IBrush CaseColor = Brushes.Black;

        /// <summary>
        /// Цвет кругов огней
        /// </summary>
        private readonly IBrush CircleColor = Brushes.Black;

        /// <summary>
        /// Размер огней - процент от ширины светофора
        /// </summary>
        private const double LightsRadusPercent = 0.85;

        /// <summary>
        /// Ширина линии корпуса
        /// </summary>
        private const double CaseLinesWidth = 5; 

        /// <summary>
        /// Ширина линии кругов
        /// </summary>
        private const double CirclesLinesWidth = 2;

        /// <summary>
        /// Радиус светодиода
        /// </summary>
        private const double LedRadius = 2;

        /// <summary>
        /// Расстояние между светодиодами
        /// </summary>
        private const double LedSpacing = 1.5;

        /// <summary>
        /// Включённый красный огонь
        /// </summary>
        private readonly IBrush RedLightOnColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0000");

        /// <summary>
        /// Включённый жёлтый огонь
        /// </summary>
        private readonly IBrush YellowLightOnColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFF00");

        /// <summary>
        /// Включённый зелёный огонь
        /// </summary>
        private readonly IBrush GreenLightOnColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00FF00");

        /// <summary>
        /// Выключенный красный огонь
        /// </summary>
        private readonly IBrush RedLightOffColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#500000");

        /// <summary>
        /// Выключенный жёлтый огонь
        /// </summary>
        private readonly IBrush YellowLightOffColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#666300");

        /// <summary>
        /// Выключенный зелёный огонь
        /// </summary>
        private readonly IBrush GreenLightOffColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#035200");

        /// <summary>
        /// Цвет подложки красного огня
        /// </summary>
        private readonly IBrush BckgColorRed = (SolidColorBrush)new BrushConverter().ConvertFrom("#280000");

        /// <summary>
        /// Цвет подложки жёлтого огня
        /// </summary>
        private readonly IBrush BckgColorYellow = (SolidColorBrush)new BrushConverter().ConvertFrom("#212100");

        /// <summary>
        /// Цвет подложки зелёного огня
        /// </summary>
        private readonly IBrush BckgColorGreen = (SolidColorBrush)new BrushConverter().ConvertFrom("#002C04");


        private readonly IBrush CaseBckgColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");


        #region Управление огнями

        /// <summary>
        /// Свойство управления красным огнём
        /// </summary>
        public static readonly AttachedProperty<bool> IsRedOnProperty = AvaloniaProperty.RegisterAttached<TrafficLightsCanvas, Interactive, bool>(nameof(IsRedOn));

        /// <summary>
        /// Горит-ли красный огонь
        /// </summary>
        public bool IsRedOn
        {
            get { return GetValue(IsRedOnProperty); }
            set { SetValue(IsRedOnProperty, value); }
        }

        /// <summary>
        /// Свойство управления жёлтым огнём
        /// </summary>
        public static readonly AttachedProperty<bool> IsYellowOnProperty = AvaloniaProperty.RegisterAttached<TrafficLightsCanvas, Interactive, bool>(nameof(IsYellowOn));

        /// <summary>
        /// Горит-ли красный огонь
        /// </summary>
        public bool IsYellowOn
        {
            get { return GetValue(IsYellowOnProperty); }
            set { SetValue(IsYellowOnProperty, value); }
        }

        /// <summary>
        /// Свойство управления жёлтым огнём
        /// </summary>
        public static readonly AttachedProperty<bool> IsGreenOnProperty = AvaloniaProperty.RegisterAttached<TrafficLightsCanvas, Interactive, bool>(nameof(IsGreenOn));

        /// <summary>
        /// Горит-ли красный огонь
        /// </summary>
        public bool IsGreenOn
        {
            get { return GetValue(IsGreenOnProperty); }
            set { SetValue(IsGreenOnProperty, value); }
        }

        #endregion


        public TrafficLightsCanvas()
        {
            InitializeComponent();

            IsRedOnProperty.Changed.Subscribe(x => HandleLightChangedChanged(x.Sender, x.NewValue.GetValueOrDefault<bool>()));
            IsYellowOnProperty.Changed.Subscribe(x => HandleLightChangedChanged(x.Sender, x.NewValue.GetValueOrDefault<bool>()));
            IsGreenOnProperty.Changed.Subscribe(x => HandleLightChangedChanged(x.Sender, x.NewValue.GetValueOrDefault<bool>()));
        }

        /// <summary>
        /// Перерисовать при изменении состояния огней
        /// </summary>
        private void HandleLightChangedChanged(IAvaloniaObject element, bool newValue)
        {
            InvalidateVisual();
        }

        /// <summary>
        /// Метод отрисовки
        /// </summary>
        public override void Render(DrawingContext context)
        {
            // Ширина и высота прямоугольника  
            var width = Width;
            var height = Height;

            // Горизонтальный центр
            var centerX = width / 2;

            // Вертикальные центры огней
            var centerRedY = 1 * height / 4;
            var centerYellowY = 2 * height / 4;
            var centerGreenY = 3 * height / 4;

            // Радиусы контуров огней
            var lightsContoursRaduses = LightsRadusPercent * width / 2;

            // Рисуем корпус светофора
            var casePen = new Pen(CaseColor, CaseLinesWidth, lineCap: PenLineCap.Square);
            context.DrawRectangle(CaseBckgColor, casePen, new Rect(0, 0, width, height));

            // Рисуем круги
            var circlePen = new Pen(CircleColor, CirclesLinesWidth, lineCap: PenLineCap.Square);

            DrawCircle(context, centerX, centerRedY, lightsContoursRaduses, circlePen, BckgColorRed); // Контур красного огня
            DrawLights(context, centerX - lightsContoursRaduses, centerRedY - lightsContoursRaduses, 2 * lightsContoursRaduses, IsRedOn ? RedLightOnColor : RedLightOffColor); // Красный огонь

            DrawCircle(context, centerX, centerYellowY, lightsContoursRaduses, circlePen, BckgColorYellow); // Контур жёлтого огня
            DrawLights(context, centerX - lightsContoursRaduses, centerYellowY - lightsContoursRaduses, 2 * lightsContoursRaduses, IsYellowOn ? YellowLightOnColor : YellowLightOffColor); // Жётый огонь

            DrawCircle(context, centerX, centerGreenY, lightsContoursRaduses, circlePen, BckgColorGreen); // Контур зелёного огня            
            DrawLights(context, centerX - lightsContoursRaduses, centerGreenY - lightsContoursRaduses, 2 * lightsContoursRaduses, IsGreenOn ? GreenLightOnColor : GreenLightOffColor); // Зелёный огонь

            base.Render(context);
        }

        /// <summary>
        /// Рисование круга
        /// </summary>
        private void DrawCircle(DrawingContext context, double x, double y, double radius, Pen pen, IBrush bckgColor)
        {
            context.DrawEllipse(bckgColor, pen, new Point(x, y), radius, radius);
        }

        /// <summary>
        /// Рисование светодиода
        /// </summary>
        private void DrawLed(DrawingContext context, double x, double y, IBrush ledColor)
        {
            context.DrawEllipse(ledColor, new Pen(ledColor, 1, lineCap: PenLineCap.Square), new Point(x, y), LedRadius, LedRadius);
        }

        /// <summary>
        /// Рисование светодиоидов в рамке огней
        /// </summary>
        private void DrawLights(DrawingContext context, double x, double y, double side, IBrush lightColor)
        {
            for (double yPos = y; yPos <= y + side; yPos += 2 * LedRadius + LedSpacing)
            {
                for (double xPos = x; xPos <= x + side; xPos += 2 * LedRadius + LedSpacing)
                {
                    if (Math.Pow(xPos - x - 0.5 * side, 2) + Math.Pow(yPos - y - 0.5 * side, 2) <= Math.Pow(0.5 * (side - LedRadius), 2))
                    {
                        DrawLed(context, xPos, yPos, lightColor);
                    }
                }
            }
        }
    }
}

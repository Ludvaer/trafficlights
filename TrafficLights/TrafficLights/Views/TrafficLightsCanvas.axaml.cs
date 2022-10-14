using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Reflection.PortableExecutable;

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

        public TrafficLightsCanvas()
        {
            InitializeComponent();
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
            context.DrawRectangle(casePen, new Rect(0, 0, width, height));

            // Рисуем круги
            var circlePen = new Pen(CircleColor, CirclesLinesWidth, lineCap: PenLineCap.Square);

            // Рисуем контур красного огня
            DrawLights(context, centerX - lightsContoursRaduses, centerRedY - lightsContoursRaduses, 2 * lightsContoursRaduses, Brushes.Red);
            DrawCircle(context, centerX, centerRedY, lightsContoursRaduses, circlePen);

            // Рисуем контур жёлтого огня
            DrawLights(context, centerX - lightsContoursRaduses, centerYellowY - lightsContoursRaduses, 2 * lightsContoursRaduses, Brushes.Yellow);
            DrawCircle(context, centerX, centerYellowY, lightsContoursRaduses, circlePen);

            // Рисуем контур зелёного огня
            DrawLights(context, centerX - lightsContoursRaduses, centerGreenY - lightsContoursRaduses, 2 * lightsContoursRaduses, Brushes.Green);
            DrawCircle(context, centerX, centerGreenY, lightsContoursRaduses, circlePen);

            base.Render(context);
        }

        /// <summary>
        /// Рисование круга
        /// </summary>
        private void DrawCircle(DrawingContext context, double x, double y, double radius, Pen pen)
        {
            context.DrawEllipse(Brushes.Transparent, pen, new Point(x, y), radius, radius);
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

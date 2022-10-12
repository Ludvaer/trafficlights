using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Reflection.PortableExecutable;

namespace TrafficLights.Views
{
    public partial class TrafficLightsCanvas : UserControl
    {
        /// <summary>
        /// Цвет корпуса
        /// </summary>
        public readonly IBrush CaseColor = Brushes.Black;

        /// <summary>
        /// Цвет кругов огней
        /// </summary>
        public readonly IBrush CircleColor = Brushes.Black;

        /// <summary>
        /// Размер огней - процент от ширины светофора
        /// </summary>
        public const double LightsRadusPercent = 0.85;

        /// <summary>
        /// Ширина линии корпуса
        /// </summary>
        private const double CaseLinesWidth = 5; 

        /// <summary>
        /// Ширина линии кругов
        /// </summary>
        public const double CirclesLinesWidth = 2;

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

            // Горизонтальный центр треугольника
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
            DrawCircle(context, centerX, centerRedY, lightsContoursRaduses, circlePen);

            // Рисуем контур жёлтого огня
            DrawCircle(context, centerX, centerYellowY, lightsContoursRaduses, circlePen);

            // Рисуем контур зелёного огня
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
    }
}

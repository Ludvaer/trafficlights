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
            var width = Width;
            var height = Height;

            // Рисуем корпус светофора
            var casePen = new Pen(CaseColor, CaseLinesWidth, lineCap: PenLineCap.Square);
            context.DrawRectangle(casePen, new Rect(0, 0, width, height));

            // Рисуем круги
            var circlePen = new Pen(CircleColor, CirclesLinesWidth, lineCap: PenLineCap.Square);
            DrawCircle(context, 50, 50, 10, circlePen);

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

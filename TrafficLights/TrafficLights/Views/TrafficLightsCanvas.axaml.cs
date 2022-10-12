using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace TrafficLights.Views
{
    public partial class TrafficLightsCanvas : UserControl
    {
        public TrafficLightsCanvas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод отрисовки
        /// </summary>
        public override void Render(DrawingContext context)
        {
            var point1 = new Point(0, 0);
            var point2 = new Point(100, 100);

            var pen = new Pen(Brushes.Green, 1, lineCap: PenLineCap.Square);

            context.DrawLine(pen, point1, point2);

            base.Render(context);
        }
    }
}

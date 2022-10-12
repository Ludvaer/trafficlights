using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Reflection.PortableExecutable;

namespace TrafficLights.Views
{
    public partial class TrafficLightsCanvas : UserControl
    {
        /// <summary>
        /// ���� �������
        /// </summary>
        public readonly IBrush CaseColor = Brushes.Black;

        /// <summary>
        /// ���� ������ �����
        /// </summary>
        public readonly IBrush CircleColor = Brushes.Black;

        /// <summary>
        /// ������ ����� �������
        /// </summary>
        private const double CaseLinesWidth = 5; 

        /// <summary>
        /// ������ ����� ������
        /// </summary>
        public const double CirclesLinesWidth = 2;

        public TrafficLightsCanvas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ����� ���������
        /// </summary>
        public override void Render(DrawingContext context)
        {
            var width = Width;
            var height = Height;

            // ������ ������ ���������
            var casePen = new Pen(CaseColor, CaseLinesWidth, lineCap: PenLineCap.Square);
            context.DrawRectangle(casePen, new Rect(0, 0, width, height));

            // ������ �����
            var circlePen = new Pen(CircleColor, CirclesLinesWidth, lineCap: PenLineCap.Square);
            DrawCircle(context, 50, 50, 10, circlePen);

            base.Render(context);
        }

        /// <summary>
        /// ��������� �����
        /// </summary>
        private void DrawCircle(DrawingContext context, double x, double y, double radius, Pen pen)
        {
            context.DrawEllipse(Brushes.Transparent, pen, new Point(x, y), radius, radius);
        }
    }
}

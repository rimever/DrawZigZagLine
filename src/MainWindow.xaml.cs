using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawZigZagLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            {
                var polyLine = new Polyline();
                var points = new PointCollection()
                {
                    new Point(100, 100),
                    new Point(700, 500)
                };
                polyLine.Points = points;
                polyLine.Stroke = Brushes.Red;
                Canvas.Children.Add(polyLine);
            }
            {
                var polyLine = new Polyline();
                var points = new PointCollection();
                foreach (var point in EnumerableZigZagPoints(new Point(100, 100), new Point(700, 500)))
                {
                    points.Add(point);
                }
                polyLine.Points = points;
                polyLine.Stroke = Brushes.Blue;
                Canvas.Children.Add(polyLine);
            }
        }

        private IEnumerable<Point> EnumerableZigZagPoints(Point startPoint, Point endPoint)
        {
            var span = 20d;
            var count = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2)) /
                        span;
            var spanPoints = new List<Point>();
            for (int i = 0; i < count; i++)
            {
                var point = new Point
                {
                    X = startPoint.X + (endPoint.X - startPoint.X) * i / count,
                    Y = startPoint.Y + (endPoint.Y - startPoint.Y) * i / count
                };
                spanPoints.Add(point);
            }

            yield return startPoint;

            for (int i = 0; i < spanPoints.Count - 1; i++)
            {
                var previousPoint = spanPoints[i];
                var nextPoint = spanPoints[i + 1];
                var cx = nextPoint.X - previousPoint.X;
                var cy = nextPoint.Y - previousPoint.Y;
                var angle = 60 * Math.PI / 180;
                var x = cx * (float)Math.Cos(angle) - cy * (float)Math.Sin(angle) + previousPoint.X;
                var y = cx * (float)Math.Sin(angle) + cy * (float)Math.Cos(angle) + previousPoint.Y;
                yield return new Point(x, y);
                yield return nextPoint;
            }
            yield return endPoint;
        }
    }
}

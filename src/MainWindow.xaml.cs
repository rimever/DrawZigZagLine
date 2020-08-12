#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#endregion

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
                var points = new PointCollection
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

            var diffZigZagPoint = spanPoints.Count >= 2
                ? GetZigZagPoint(new Point()
                    ,new Point()
                    {
                        X = spanPoints.Skip(1).First().X - spanPoints.First().X,
                        Y = spanPoints.Skip(1).First().Y - spanPoints.First().Y
                    })
                : new Point();
            // 「2点の中間地点」と「折れ点」の中間距離を割り出しておき、その分、ずらすのに使う
            var slideZigZagPoint = new Point()
            {
                X = ((spanPoints.Skip(1).First().X - spanPoints.First().X) / 2 - diffZigZagPoint.X) / 2,
                Y = ((spanPoints.Skip(1).First().Y - spanPoints.First().Y) / 2 - diffZigZagPoint.Y) / 2
            };

            yield return startPoint;
            for (int i = 0; i < spanPoints.Count - 1; i++)
            {
                var previousPoint = spanPoints[i];
                var nextPoint = spanPoints[i + 1];
                yield return new Point()
                {
                    X = previousPoint.X + diffZigZagPoint.X + slideZigZagPoint.X,
                    Y = previousPoint.Y + diffZigZagPoint.Y + slideZigZagPoint.Y
                };
                yield return new Point()
                {
                    X = nextPoint.X + slideZigZagPoint.X,
                    Y = nextPoint.Y + slideZigZagPoint.Y,
                };
            }

            yield return endPoint;
        }

        /// <summary>
        /// 2点の間のジグザグ線の折れ点となる座標を算出します
        /// </summary>
        /// <param name="previousPoint"></param>
        /// <param name="nextPoint"></param>
        /// <returns></returns>
        private static Point GetZigZagPoint(Point previousPoint, Point nextPoint)
        {
            var cx = nextPoint.X - previousPoint.X;
            var cy = nextPoint.Y - previousPoint.Y;
            var angle = 60 * Math.PI / 180;
            var x = cx * (float) Math.Cos(angle) - cy * (float) Math.Sin(angle) + previousPoint.X;
            var y = cx * (float) Math.Sin(angle) + cy * (float) Math.Cos(angle) + previousPoint.Y;
            return new Point(x, y);
        }
    }
}
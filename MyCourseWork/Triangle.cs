using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    class Triangle : Shape
    {
        private float a;
        private float b;
        private float c;

        public float A
        {
            get => a;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The side should be positive number!");
                }

                a = value;
            }
        }
        public float B
        {
            get => b;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The side should be positive number!");
                }

                b = value;
            }
        }
        public float C
        {
            get => c;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The side should be positive number!");
                }

                c = value;
            }
        }

        public Triangle(float a, float b, float c, float x, float y, int id, Color color) 
            : base(x, y, id, color)
        {
            IsPossibleTriangle(a, b, c);
            this.a = a;
            this.b = b;
            this.c = c;
        }

        private void IsPossibleTriangle(float a, float b, float c)
        {
            if (a >= b + c)
            {
                throw new Exception("Triangle with this sides cannot exists!");
            }
            else if (b >= a + c)
            {
                throw new Exception("Triangle with this sides cannot exists!");
            }
            else if (c >= a + b)
            {
                throw new Exception("Triangle with this sides cannot exists!");
            }
        }

        public override double CalculateSurface()
        {
            double p = (a + b + c) / 2;
            surface = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return surface;
        }
        public override double CalculatePerimeter()
        {
            perimeter = a + b + c;
            return perimeter;
        }

        public override void Draw(Graphics graphics, float x, float y)
        {
            var h = 2 * Surface / c;
            var distanceToA = Math.Sqrt((b * b) - (h * h));
            var distanceToB = Math.Sqrt((a * a) - (h * h));
            var pointC = new Point((int)x, (int)y);
            var pointA = new Point((int)(x - distanceToA), (int)(y + h));
            var pointB = new Point((int)(x + distanceToB), (int)(y + h));

            if (a > c && a > b)
            {
                pointA.X = (int)(X + distanceToA);
                pointB.X = (int)(pointA.X + c);
            }
            else if (b > c && b > a)
            {
                pointB.X = (int)(X - distanceToB);
                pointA.X = (int)(pointB.X - c);
            }

            var points = new Point[] { pointA, pointB, pointC };

            graphics.DrawPolygon(Pen, points);
        }

        public override void Fill(Graphics graphics, float x, float y)
        {
            var h = 2 * Surface / c;
            var distanceToA = Math.Sqrt((b * b) - (h * h));
            var distanceToB = Math.Sqrt((a * a) - (h * h));
            var pointC = new Point((int)x, (int)y);
            var pointA = new Point((int)(x - distanceToA), (int)(y + h));
            var pointB = new Point((int)(x + distanceToB), (int)(y + h));

            if (a > c && a > b)
            {
                pointA.X = (int)(X + distanceToA);
                pointB.X = (int)(pointA.X + c);
            }
            else if (b > c && b > a)
            {
                pointB.X = (int)(X - distanceToB);
                pointA.X = (int)(pointB.X - c);
            }

            var points = new Point[] { pointA, pointB, pointC };

            graphics.FillPolygon(Brush, points);
        }

        public override bool Contains(Point point)
        {
            var h = 2 * Surface / c;
            var distanceToA = Math.Sqrt((b * b) - (h * h));
            var distanceToB = Math.Sqrt((a * a) - (h * h));
            var pointA = new Point((int)(X - distanceToA), (int)(Y + h));
            var pointB = new Point((int)(X + distanceToB), (int)(Y + h));

            if (a > c && a > b)
            {
                pointA.X = (int)(X + distanceToA);
                pointB.X = (int)(pointA.X + c);
            }
            else if (b > c && b > a)
            {
                pointB.X = (int)(X - distanceToB);
                pointA.X = (int)(pointB.X - c);
            }

            bool result = false;

            if (point.Y + 88 < pointA.Y && point.Y + 88 > Y)
            {
                if (point.X <= X)
                {
                    var ratio = (X - pointA.X) / h;
                    var diff = pointA.Y - (point.Y + 88);
                    result = pointA.X + (diff * ratio) <= point.X;

                    if (result && pointB.X < X)
                    {
                        var diff2 = (point.Y + 88) - Y;
                        var ratio2 = (X - pointB.X) / h;
                        result = X - (diff2 * ratio2) >= point.X;
                        return result;
                    }

                    return result;
                }
                else if (point.X >= X)
                {
                    var ratio = (pointB.X - X) / h;
                    var diff = pointB.Y - (point.Y + 88);
                    result = pointB.X - (diff * ratio) >= point.X;

                    if (result && pointA.X > X)
                    {
                        var diff2 = (point.Y + 88) - Y;
                        var ratio2 = (pointA.X - X) / h;
                        result = X + (diff2 * ratio2) <= point.X;
                        return result;
                    }

                    return result;
                }
            }

            return result;
        }
    }
}

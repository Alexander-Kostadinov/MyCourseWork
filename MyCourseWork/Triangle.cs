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

        public override void Draw(Graphics graphics, float x, float y , Color color)
        {
            var h = 2 * Surface / c;
            var distanceToA = Math.Sqrt(b * b - h * h);
            var distanceToB = Math.Sqrt(a * a - h * h);
            var pointC = new Point((int)x, (int)y);
            var pointA = new Point((int)(x - distanceToA), (int)(y + h));
            var pointB = new Point((int)(x + distanceToB), (int)(y + h));

            if (a > c && a > b)
            {
                pointA.X = (int)(x + distanceToA);
                pointB.X = (int)(x + c + distanceToA);
            }
            if (b > c && b > a)
            {
                pointB.X = (int)(x - distanceToB);
                pointA.X = (int)(x - (c + distanceToB));
            }

            var points = new Point[] { pointA, pointB, pointC };

            graphics.DrawPolygon(Pen, points);
        }

        public override void Fill(Graphics graphics, float x, float y, Color color)
        {
            var h = 2 * Surface / c;
            var distanceToA = Math.Sqrt(b * b - h * h);
            var distanceToB = Math.Sqrt(a * a - h * h);
            var pointC = new Point((int)x, (int)y);
            var pointA = new Point((int)(x - distanceToA), (int)(y + h));
            var pointB = new Point((int)(x + distanceToB), (int)(y + h));

            if (a > c && a > b)
            {
                pointA.X = (int)(x + distanceToA);
                pointB.X = (int)(x + c + distanceToA);
            }
            if (b > c && b > a)
            {
                pointB.X = (int)(x - distanceToB);
                pointA.X = (int)(x - (c + distanceToB));
            }

            var points = new Point[] { pointA, pointB, pointC };

            graphics.FillPolygon(Brush, points);
        }

        public override bool Contains(Point point)
        {
            var x = X;
            var y = Y;
            var h = 2 * Surface / c;
            var distanceToA = Math.Sqrt(b * b - h * h);
            var distanceToB = Math.Sqrt(a * a - h * h);
            var pointB = new Point((int)(x + distanceToB), (int)(y + h));
            var pointA = new Point((int)(x - distanceToA), (int)(y + h));
            bool result = false;
            var diff = pointA.Y - (point.Y + 87);

            if (pointA.X < X && point.X < X)
            {
                result = point.Y + 87 <= pointA.Y && point.Y + 87 >= Y && point.X + 25 > pointA.X + diff;
            }
            else if (pointB.X > X && point.X > X)
            {
                result = point.Y + 87 <= pointB.Y && point.Y + 87 >= Y && point.X < pointB.X - diff;
            }

            return result;
        }
    }
}

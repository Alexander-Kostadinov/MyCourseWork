using System;

namespace Shapes
{
    public class Rectangle : Shape
    {
        private float a;
        private float b;

        public float A
        {
            get => a;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The side must be positive number!");
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
                    throw new ArgumentException("The side must be positive number!");
                }

                b = value;
            }
        }

        public Rectangle(float a, float b, float x, float y, int id, string color) 
            : base(x, y, id, color)
        {
            A = a;
            B = b;
        }

        public override double CalculateSurface()
        {
            surface = a * b;
            return surface;
        }
        public override double CalculatePerimeter()
        {
            perimeter = 2 * a + 2 * b;
            return perimeter;
        }

        public override Point[] GetPoints(float x, float y)
        {
            Point pointA = new Point((int)x, (int)y);
            Point pointB = new Point((int)(x + a), (int)y);
            Point pointC = new Point((int)(x + a), (int)(y + b));
            Point pointD = new Point((int)x, (int)(y + b));

            Point[] points = new Point[] { pointA, pointB, pointC, pointD };
            return points;
        }

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + a &&
                Y <= point.Y && point.Y <= Y + b;
        }
    }
}

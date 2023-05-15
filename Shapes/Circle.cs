using System;

namespace Shapes
{
    public class Circle : Shape
    {
        private float radius;

        public float Radius
        {
            get => radius;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The radius must be positive number!");
                }

                radius = value;
            }
        }

        public Circle(float radius, float x, float y, int id, string color) 
            : base(x, y, id, color)
        {
            Radius = radius;
        }

        public override double CalculateSurface()
        {
            surface = Math.PI * radius * radius;
            return surface;
        }
        public override double CalculatePerimeter()
        {
            perimeter = 2 * Math.PI * radius;
            return perimeter;
        }

        public override Point[] GetPoints(float x, float y)
        {
            Point pointA = new Point((int)x, (int)y);
            Point pointB = new Point((int)(x + radius), (int)y);
            Point pointC = new Point((int)(x + radius), (int)(y + radius));
            Point pointD = new Point((int)x, (int)(y + radius));

            Point[] points = new Point[] { pointA, pointB, pointC, pointD };
            return points;
        }

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + radius &&
                Y <= point.Y && point.Y <= Y + radius;
        }
    }
}

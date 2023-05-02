using System;

namespace Shapes
{
    public class Circle : Shape
    {
        public Circle(float radius, float x, float y, int id, string color) 
            : base(x, y, id, color)
        {
            FirstSide = radius;
        }

        public override double CalculateSurface()
        {
            surface = Math.PI * firstSide * firstSide;
            return surface;
        }
        public override double CalculatePerimeter()
        {
            perimeter = 2 * Math.PI * firstSide;
            return perimeter;
        }

        public override Point[] GetPoints(float x, float y)
        {
            Point pointA = new Point((int)x, (int)y);
            Point pointB = new Point((int)(x + firstSide), (int)y);
            Point pointC = new Point((int)(x + firstSide), (int)(y + firstSide));
            Point pointD = new Point((int)x, (int)(y + firstSide));

            Point[] points = new Point[] { pointA, pointB, pointC, pointD };
            return points;
        }

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + firstSide &&
                Y <= point.Y + 88 && point.Y + 88 <= Y + firstSide;
        }
    }
}

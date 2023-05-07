namespace Shapes
{
    public class Rectangle : Shape
    {
        public Rectangle(float a, float b, float x, float y, int id, string color) 
            : base(x, y, id, color)
        {
            FirstSide = a;
            SecondSide = b;
        }

        public override double CalculateSurface()
        {
            surface = firstSide * secondSide;
            return surface;
        }
        public override double CalculatePerimeter()
        {
            perimeter = 2 * firstSide + 2 * secondSide;
            return perimeter;
        }

        public override Point[] GetPoints(float x, float y)
        {
            Point pointA = new Point((int)x, (int)y);
            Point pointB = new Point((int)(x + firstSide), (int)y);
            Point pointC = new Point((int)(x + firstSide), (int)(y + secondSide));
            Point pointD = new Point((int)x, (int)(y + secondSide));

            Point[] points = new Point[] { pointA, pointB, pointC, pointD };
            return points;
        }

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + firstSide &&
                Y <= point.Y && point.Y <= Y + secondSide;
        }
    }
}

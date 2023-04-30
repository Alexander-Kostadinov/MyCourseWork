using System;

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

        public override Point[] Draw(float x, float y)
        {
            throw new NotImplementedException();
        }

        public override Point[] Fill(float x, float y)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(Point point)
        {
            return
                X <= point.X + 1 && point.X + 1 <= X + firstSide &&
                Y <= point.Y + 88 && point.Y + 88 <= Y + secondSide;
        }
    }
}

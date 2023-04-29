using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                X <= point.X && point.X <= X + firstSide &&
                Y <= point.Y + 88 && point.Y + 88 <= Y + firstSide;
        }
    }
}

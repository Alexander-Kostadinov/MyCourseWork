using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes
{
    public class Rectangle : Shape
    {
        public Rectangle(float a, float b, float x, float y, int id, Color color) 
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

        public override void Draw(Graphics graphics, float x, float y)
            => graphics.DrawRectangle(Pen, x, y, firstSide, secondSide);

        public override void Fill(Graphics graphics, float x, float y)
            => graphics.FillRectangle(Brush, x, y, firstSide, secondSide);

        public override bool Contains(Point point)
        {
            return
                X <= point.X + 1 && point.X + 1 <= X + firstSide &&
                Y <= point.Y + 88 && point.Y + 88 <= Y + secondSide;
        }
    }
}

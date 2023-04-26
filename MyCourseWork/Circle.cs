using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    class Circle : Shape
    {
        public Circle(float radius, float x, float y, int id, Color color) 
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

        public override void Draw(Graphics graphics, float x, float y) 
            => graphics.DrawEllipse(Pen, x, y, firstSide, firstSide);

        public override void Fill(Graphics graphics, float x, float y) 
            => graphics.FillEllipse(Brush, x, y, firstSide, firstSide);

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + firstSide &&
                Y <= point.Y + 87 && point.Y + 87 <= Y + firstSide;
        }
    }
}

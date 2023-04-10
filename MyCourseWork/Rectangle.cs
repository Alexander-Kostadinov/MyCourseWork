using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    class Rectangle : Shape
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

        public Rectangle(float a, float b, float x, float y, int id, Color color) 
            : base(x, y, id, color)
        {
            this.a = a;
            this.b = b;
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

        public override void Draw(Graphics graphics, float x, float y, Color color)
            => graphics.DrawRectangle(Pen, x, y, a, b);

        public override void Fill(Graphics graphics, float x, float y, Color color)
            => graphics.FillRectangle(Brush, x, y, a, b);

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + A &&
                Y <= point.Y + 87 && point.Y + 87 <= Y + B;
        }
    }
}

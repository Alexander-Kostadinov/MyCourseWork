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
        private float radius;

        public float Radius
        {
            get => radius;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The radius should be positive number!");
                }

                radius = value;
            }
        }

        public Circle(float radius, float x, float y, int id, Color color) 
            : base(x, y, id, color)
        {
            this.radius = radius;
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

        public override void Draw(Graphics graphics, float x, float y, Color color) 
            => graphics.DrawEllipse(Pen, x, y, radius, radius);

        public override void Fill(Graphics graphics, float x, float y, Color color) 
            => graphics.FillEllipse(Brush, x, y, radius, radius);

        public override bool Contains(Point point)
        {
            return
                X <= point.X && point.X <= X + Radius &&
                Y <= point.Y + 87 && point.Y + 87 <= Y + Radius;
        }
    }
}

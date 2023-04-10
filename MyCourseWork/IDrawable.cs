using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public interface IDrawable
    {
        float X { get; set; }
        float Y { get; set; }
        int ID { get; set; }
        Color Color { get; set; }
        double Surface { get; }
        double Perimeter { get; }

        double CalculateSurface();
        double CalculatePerimeter();

        bool Contains(Point point);
        void Draw(Graphics graphics, float x, float y, Color color);
        void Fill(Graphics graphics, float x, float y, Color color);
    }
}

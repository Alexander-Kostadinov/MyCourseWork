using Shapes;
using System.Drawing;

namespace MyCourseWork
{
    public class Command
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public IDrawable Item { get; set; }
    }
}

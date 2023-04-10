using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyCourseWork
{
    public class Command
    {
        public string Name { get; set; }
        public IDrawable Item { get; set; }
        public Color Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}

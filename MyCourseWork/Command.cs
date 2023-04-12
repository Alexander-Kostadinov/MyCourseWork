using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyCourseWork
{
    public class Command : ICommand
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public IDrawable Item { get; set; }
        public List<IDrawable> Shapes { get; set; }
        public List<Command> UndoCommands { get; set; }
        public List<Command> RedoCommands { get; set; }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }
    }
}

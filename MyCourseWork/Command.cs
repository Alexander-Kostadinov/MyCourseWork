using System;
using Shapes;
using System.Drawing;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Command
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public IDrawable Item { get; set; }
        protected List<IDrawable> Shapes { get; set; }
        protected List<Command> UndoCommands { get; set; }
        protected List<Command> RedoCommands { get; set; }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }
    }
}

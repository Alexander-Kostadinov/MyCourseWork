using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public abstract class Commands : ICommands
    {
        protected Command Command { get; set; }
        protected List<IDrawable> Shapes { get; set; }
        protected List<Command> UndoCommands { get; set; }
        protected List<Command> RedoCommands { get; set; }

        protected Commands(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
        {
            Shapes = shapes;
            Command = new Command();
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

        public abstract void Execute();
    }
}

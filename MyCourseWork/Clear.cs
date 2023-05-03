using System;
using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Clear : Commands
    {
        public Clear(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
            : base(undoCommands, redoCommands, shapes) { }

        public override void Execute()
        {
            Shapes.Clear();
            UndoCommands.Clear();
            RedoCommands.Clear();
        }
    }
}

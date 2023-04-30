using System;
using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Clear : Command
    {
        public Clear(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
        {
            Shapes = shapes;
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

        public override void Execute()
        {
            Shapes.Clear();
            UndoCommands.Clear();
            RedoCommands.Clear();
        }
    }
}

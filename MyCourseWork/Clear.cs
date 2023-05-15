using System;
using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Clear : Command
    {
        private List<ICommand> UndoCommands { get; set; }
        private List<ICommand> RedoCommands { get; set; }

        public Clear(IDrawable shape, List<IDrawable> shapes, 
            List<ICommand> undoCommands, List<ICommand> redoCommands) 
            : base(shape, shapes)
        {
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

        public override void Execute()
        {
            Shapes.Clear();
            UndoCommands.Clear();
            RedoCommands.Clear();
        }
        public override void UndoExecute()
        {
            throw new NotImplementedException();
        }
    }
}

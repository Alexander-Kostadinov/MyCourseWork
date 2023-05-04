using System;
using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Clear : Command
    {
        private List<ICommand> UndoCommands { get; set; }
        private List<ICommand> RedoCommands { get; set; }
        private List<ISerializable> Serializables { get; set; }

        public Clear(IDrawable shape, List<IDrawable> shapes, 
            List<ICommand> undoCommands, List<ICommand> redoCommands, List<ISerializable> serializables) 
            : base(shape, shapes)
        {
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
            Serializables = serializables;
        }

        public override void Execute()
        {
            Shapes.Clear();
            UndoCommands.Clear();
            RedoCommands.Clear();
            Serializables.Clear();
        }
        public override void UndoExecute()
        {
            throw new NotImplementedException();
        }
    }
}

using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Add : Commands
    {
        private IDrawable Shape { get; set; }

        public Add(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes, IDrawable shape) 
            : base(undoCommands, redoCommands, shapes)
        {
            Shape = shape;
        }

        public override void Execute()
        {
            Shapes.Add(Shape);
            RedoCommands.Clear();
            Command.Name = "Add";
            Command.Item = Shape;
            UndoCommands.Add(Command);

            Command = new Command();
        }
    }
}

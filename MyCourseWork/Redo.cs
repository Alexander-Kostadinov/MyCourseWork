using Shapes;
using System.Linq;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Redo : Commands
    {
        public Redo(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes) 
            : base(undoCommands, redoCommands, shapes) { }

        public override void Execute()
        {
            var lastCommand = RedoCommands.LastOrDefault();

            if (lastCommand == null)
            {
                return;
            }

            switch (lastCommand.Name)
            {
                case "Add":
                    Shapes.Add(lastCommand.Item);
                    break;
                case "Fill":
                    var colorToChange = Shapes.Where(x => 
                    x.ID == lastCommand.Item.ID || x.ID == lastCommand.Item.ID * -1).FirstOrDefault();

                    if (colorToChange == null)
                    {
                        break;
                    }

                    colorToChange.Color = lastCommand.Item.Color;
                    break;
                case "Move":
                    var shapeToMove = Shapes.Where(x =>
                    x.ID == lastCommand.Item.ID || x.ID == lastCommand.Item.ID * -1).FirstOrDefault();

                    if (shapeToMove == null)
                    {
                        break;
                    }

                    shapeToMove.X = lastCommand.Item.X;
                    shapeToMove.Y = lastCommand.Item.Y;
                    break;
                case "Remove":
                    Shapes.Remove(lastCommand.Item);
                    break;

                default:
                    break;
            }

            UndoCommands.Add(lastCommand);
            RedoCommands.Remove(lastCommand);
        }
    }
}

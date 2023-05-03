using Shapes;
using System.Linq;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Undo : Commands
    {
        public Undo(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes) 
            : base(undoCommands, redoCommands, shapes) { }

        public override void Execute()
        {
            var lastCommand = UndoCommands.LastOrDefault();

            if (lastCommand == null)
            {
                return;
            }

            switch (lastCommand.Name)
            {
                case "Add":
                    Shapes.Remove(lastCommand.Item);
                    break;
                case "Fill":
                    var colorToChange = Shapes.Where(x =>
                    x.ID == lastCommand.Item.ID || x.ID == lastCommand.Item.ID * -1).FirstOrDefault();

                    if (colorToChange == null)
                    {
                        break;
                    }

                    colorToChange.Color = lastCommand.Color.Name;
                    break;

                case "Move":
                    var shapeToMove = Shapes.Where(x =>
                    x.ID == lastCommand.Item.ID || x.ID == lastCommand.Item.ID * -1).FirstOrDefault();

                    if (shapeToMove == null)
                    {
                        break;
                    }

                    shapeToMove.X = lastCommand.X;
                    shapeToMove.Y = lastCommand.Y;
                    break;

                case "Remove":
                    Shapes.Add(lastCommand.Item);
                    break;

                default:
                    break;
            }

            RedoCommands.Add(lastCommand);
            UndoCommands.Remove(lastCommand);
        }     
    }
}

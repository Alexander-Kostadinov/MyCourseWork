using Shapes;
using System.Collections.Generic;
using System.Linq;

namespace MyCourseWork
{
    public class Redo : Command
    {
        public Redo(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
        {
            Shapes = shapes;
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

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

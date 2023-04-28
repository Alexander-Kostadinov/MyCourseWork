using System;
using Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public class Undo : Command
    {
        public Undo(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
        {
            Shapes = shapes;
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

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
                    var colorToChange = Shapes.Where(x => x.ID == lastCommand.Item.ID).FirstOrDefault();

                    if (colorToChange == null)
                    {
                        break;
                    }

                    colorToChange.Color = lastCommand.Color;
                    break;

                case "Move":
                    var shapeToMove = Shapes.Where(x => x.ID == lastCommand.Item.ID).FirstOrDefault();

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public class Remove : Command
    {
        public Command Command { get; set; }
        public List<IDrawable> SelectedShapes { get; set; }

        public Remove(Command command, List<Command> undoCommands, 
            List<IDrawable> shapes, List<IDrawable> selectedShapes)
        {
            Shapes = shapes;
            Command = command;
            UndoCommands = undoCommands;
            SelectedShapes = selectedShapes;
        }

        public override void Execute()
        {
            if (SelectedShapes.Count > 0)
            {
                foreach (var shape in Shapes)
                {
                    if (shape == SelectedShapes[SelectedShapes.Count() - 1])
                    {
                        Command.Name = "Remove";
                        Command.Item = shape;
                        UndoCommands.Add(Command);
                        Command = new Command();

                        Shapes.Remove(shape);
                        SelectedShapes.Clear();

                        break;
                    }
                }
            }  
        }
    }
}

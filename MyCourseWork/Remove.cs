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

        public Remove(List<Command> undoCommands, 
            List<IDrawable> shapes, List<IDrawable> selectedShapes)
        {
            Shapes = shapes;
            Command = new Command();
            UndoCommands = undoCommands;
            SelectedShapes = selectedShapes;
        }

        public override void Execute()
        {
            if (SelectedShapes.Count > 0)
            {
                foreach (var shape in Shapes)
                {
                    if (shape.ID == SelectedShapes[SelectedShapes.Count() - 1].ID)
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

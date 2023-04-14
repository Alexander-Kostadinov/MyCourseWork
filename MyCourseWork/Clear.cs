using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public class Clear : Command
    {
        public List<IDrawable> MovedShapes { get; set; }
        public List<IDrawable> SelectedShapes { get; set; }

        public Clear(List<Command> undoCommands, List<Command> redoCommands, 
            List<IDrawable> shapes, List<IDrawable> selectedShapes, List<IDrawable> movedShapes)
        {
            Shapes = shapes;
            MovedShapes = movedShapes;
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
            SelectedShapes = selectedShapes;
        }

        public override void Execute()
        {
            Shapes.Clear();
            UndoCommands.Clear();
            RedoCommands.Clear();
        }
    }
}

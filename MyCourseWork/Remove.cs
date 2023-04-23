using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public class Remove : Command
    {
        public Command Command { get; set; }

        public Remove(List<Command> undoCommands, List<Command> redoCommands,
            List<IDrawable> shapes)
        {
            Shapes = shapes;
            Command = new Command();
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

        public override void Execute()
        {
            if (Shapes.Count == 0)
            {
                return;
            }

            var selected = Shapes.Where(x => x.Pen.DashStyle == System.Drawing.Drawing2D.DashStyle.Dash).FirstOrDefault();

            if (selected == null)
            {
                return;
            }

            selected.Pen = new Pen(Color.Black, 2);

            Command.Name = "Remove";
            Command.Item = selected;

            RedoCommands.Clear();
            UndoCommands.Add(Command);

            Command = new Command();

            Shapes.Remove(selected);
        }
    }
}

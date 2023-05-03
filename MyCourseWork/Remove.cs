using Shapes;
using System.Linq;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Remove : Commands
    {
        public Remove(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
            : base(undoCommands, redoCommands, shapes) { }

        public override void Execute()
        {
            if (Shapes.Count == 0)
            {
                return;
            }

            var selected = Shapes.Where(x => x.ID < 0).FirstOrDefault();

            if (selected == null)
            {
                return;
            }

            selected.ID *= -1;

            Command.Name = "Remove";
            Command.Item = selected;

            RedoCommands.Clear();
            UndoCommands.Add(Command);
            Shapes.Remove(selected);

            Command = new Command();
        }
    }
}

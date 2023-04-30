using Shapes;
using System.Linq;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Remove : Command
    {
        private Command Command { get; set; }

        public Remove(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
        {
            Shapes = shapes;
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
            Command = new Command();
        }

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

            Command.Name = "Remove";
            Command.Item = selected;

            RedoCommands.Clear();
            UndoCommands.Add(Command);

            Command = new Command();

            Shapes.Remove(selected);
        }
    }
}

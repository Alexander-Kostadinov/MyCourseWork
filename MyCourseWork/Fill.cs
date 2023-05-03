using Shapes;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Fill : Commands
    {
        private Color Color { get; set; }

        public Fill(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes, Color color) 
            : base(undoCommands, redoCommands, shapes)
        {
            Color = color;
        }

        public override void Execute()
        {
            var selected = Shapes.Where(x => x.ID < 0).FirstOrDefault();

            if (selected == null)
            {
                return;
            }

            Command.Name = "Fill";
            Command.Color = Color.FromName(selected.Color);
            selected.Color = Color.ToArgb().ToString();

            var type = selected.GetType().Name;

            switch (type)
            {
                case "Circle":
                    var cloneCircle = new Circle(selected.FirstSide,
                        selected.X, selected.Y, selected.ID, selected.Color);
                    Command.Item = cloneCircle;
                    break;

                case "Rectangle":
                    var cloneRectangle = new Shapes.Rectangle(selected.FirstSide,
                        selected.SecondSide, selected.X, selected.Y, selected.ID, selected.Color);
                    Command.Item = cloneRectangle;
                    break;

                case "Triangle":
                    var cloneTriangle = new Triangle(selected.FirstSide, selected.SecondSide,
                        selected.ThirdSide, selected.X, selected.Y, selected.ID, selected.Color);
                    Command.Item = cloneTriangle;
                    break;

                default:
                    break;
            }

            RedoCommands.Clear();
            UndoCommands.Add(Command);

            Command = new Command();
        }
    }
}

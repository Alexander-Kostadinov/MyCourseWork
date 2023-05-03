using Shapes;
using System.Linq;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Moving : Commands
    {
        private int currentID;

        public int CurrentID { get { return currentID; } private set { currentID = value; } }

        public Moving(List<Command> undoCommands, List<Command> redoCommands, List<IDrawable> shapes)
            : base(undoCommands, redoCommands, shapes) { }

        public override void Execute()
        {
            var shape = Shapes.Where(x => x.ID == 0).FirstOrDefault();

            if (shape == null)
            {
                return;
            }

            var type = shape.GetType().Name;

            shape.ID = CurrentID;
            CurrentID = 0;

            if (shape.X == Command.X && shape.Y == Command.Y)
            {
                return;
            }

            switch (type)
            {
                case "Circle":
                    var cloneCircle = new Circle(shape.FirstSide, shape.X, shape.Y, shape.ID, shape.Color);
                    RedoCommands.Clear();
                    Command.Name = "Move";
                    Command.Item = cloneCircle;
                    UndoCommands.Add(Command);
                    break;

                case "Rectangle":
                    var cloneRectangle = new Rectangle(shape.FirstSide,
                        shape.SecondSide, shape.X, shape.Y, shape.ID, shape.Color);
                    RedoCommands.Clear();
                    Command.Name = "Move";
                    Command.Item = cloneRectangle;
                    UndoCommands.Add(Command);
                    break;

                case "Triangle":
                    var cloneTriangle = new Triangle(shape.FirstSide,
                        shape.SecondSide, shape.ThirdSide, shape.X, shape.Y, shape.ID, shape.Color);
                    RedoCommands.Clear();
                    Command.Name = "Move";
                    Command.Item = cloneTriangle;
                    UndoCommands.Add(Command);
                    break;
            }

            Command = new Command();
        }

        public void PreExecute(Point point)
        {
            var shape = Shapes.Where(x => x.Contains(point)).LastOrDefault();

            if (shape == null)
            {
                return;
            }

            currentID = shape.ID;
            shape.ID = 0;
            Command.X = (int)shape.X;
            Command.Y = (int)shape.Y;
        }
    }
}

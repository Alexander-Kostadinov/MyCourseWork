using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Moving : Command
    {
        public int X { get; set; }
        public int Y { get; set; }

        private int BeforeX { get; set; }
        private int BeforeY { get; set; }

        public Moving(IDrawable shape, List<IDrawable> shapes, int x, int y) 
            : base(shape, shapes)
        {
            X = x;
            Y = y;
            BeforeX = x;
            BeforeY = Y;
        }

        public override void Execute()
        {
            Shape.X = X;
            Shape.Y = Y;
        }
        public override void UndoExecute()
        {
            Shape.X = BeforeX;
            Shape.Y = BeforeY;
        }
    }
}

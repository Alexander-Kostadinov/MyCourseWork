using Shapes;
using System.Drawing;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Fill : Command
    {
        private Color Color { get; set; }
        private string ColorBefore { get; set; }

        public Fill(IDrawable shape, List<IDrawable> shapes, Color color) : base(shape, shapes)
        {
            Color = color;
            ColorBefore = Shape.Color;
        }

        public override void Execute()
        {
            Shape.Color = Color.ToArgb().ToString();
        }
        public override void UndoExecute()
        {
            Shape.Color = ColorBefore;
        }
    }
}

using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Add : Command
    {
        public Add(IDrawable shape, List<IDrawable> shapes) : base(shape, shapes) { }

        public override void Execute()
        {
            Shapes.Add(Shape);
        }
        public override void UndoExecute()
        {
            Shapes.Remove(Shape);
        }
    }
}

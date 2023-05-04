using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Remove : Command
    {
        public Remove(IDrawable shape, List<IDrawable> shapes) : base(shape, shapes) { }

        public override void Execute()
        {
            Shapes.Remove(Shape);
        }
        public override void UndoExecute()
        {
            Shapes.Add(Shape);
        }
    }
}

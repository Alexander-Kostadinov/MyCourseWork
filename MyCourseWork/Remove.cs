using Shapes;
using System.Collections.Generic;
using System.Linq;

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
            var ids = Shapes.Select(x => x.ID).ToArray();

            if (ids.Contains(Shape.ID))
            {
                Shape.ID = ids.OrderByDescending(x => x).FirstOrDefault() + 1;
            }

            Shapes.Add(Shape);
        }
    }
}

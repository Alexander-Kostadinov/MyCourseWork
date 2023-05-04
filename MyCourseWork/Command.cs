using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public abstract class Command : ICommand
    {
        protected IDrawable Shape { get; set; }
        protected List<IDrawable> Shapes { get; set; }

        protected Command(IDrawable shape, List<IDrawable> shapes)
        {
            Shape = shape;
            Shapes = shapes;
        }

        public abstract void Execute();
        public abstract void UndoExecute();
    }
}

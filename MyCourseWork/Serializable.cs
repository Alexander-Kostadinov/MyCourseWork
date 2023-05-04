using Shapes;
using System.Collections.Generic;

namespace MyCourseWork
{
    public abstract class Serializable : ISerializable
    {
        protected string Text { get; set; }
        protected List<IDrawable> Shapes { get; set; }

        protected Serializable(List<IDrawable> shapes, string text)
        {
            Text = text;
            Shapes = shapes;
        }

        public abstract string Serialize();
        public abstract void Deserialize();
    }
}

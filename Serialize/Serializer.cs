using Shapes;
using System.Collections.Generic;

namespace Serialization
{
    public abstract class Serializer : ISerializable
    {
        protected string text;

        protected string Text
        {
            get => text;
            set => text = value;
        }
        protected List<IDrawable> Shapes { get; set; }

        protected Serializer(List<IDrawable> shapes, string text)
        {
            Text = text;
            Shapes = shapes;
        }

        public abstract string Serialize();
        public abstract void Deserialize();
    }
}

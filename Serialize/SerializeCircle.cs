using Shapes;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Serialization
{
    public class SerializeCircle : Serializer
    {
        public SerializeCircle(List<IDrawable> shapes, string text) : base(shapes, text) { }

        public override string Serialize()
        {
            var circles = Shapes.Where(x => x.GetType().Name == "Circle").ToArray();

            foreach (var circle in circles)
            {
                var type = circle.GetType();
                var r = type.GetProperty("Radius").GetValue(circle);

                Text += $"Circle Id: " +
                    $"{circle.ID} X: {circle.X} Y: {circle.Y} Radius: {r} Color: {circle.Color}\n";
            }

            return Text;
        }
        public override void Deserialize()
        {
            var lines = Text.Split('\n').ToList();
            lines.RemoveAt(lines.Count() - 1);

            foreach (var line in lines)
            {
                var shape = line.Split(' ').ToArray();

                if (shape[0] == "Circle")
                {
                    Circle circle = new Circle(float.Parse(shape[8]), float.Parse(shape[4]), float.Parse(shape[6]), 
                        int.Parse(shape[2]), Color.FromName(shape[10]).Name);

                    Shapes.Add(circle);
                }
            }
        }

    }
}

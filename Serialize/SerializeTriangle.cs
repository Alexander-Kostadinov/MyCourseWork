using Shapes;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Serialization
{
    public class SerializeTriangle : Serializer
    {
        public SerializeTriangle(List<IDrawable> shapes, string text) : base(shapes, text) { }

        public override string Serialize()
        {
            var triangles = Shapes.Where(x => x.GetType().Name == "Triangle").ToArray();

            foreach (var triangle in triangles)
            {
                Text += $"Triangle Id: {triangle.ID} X: {triangle.X} Y: {triangle.Y} " +
                    $"A: {triangle.FirstSide} B: {triangle.SecondSide} C: {triangle.ThirdSide} Color: {triangle.Color}\n";
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

                if (shape[0] == "Triangle")
                {
                    var triangle = new Triangle(float.Parse(shape[8]), float.Parse(shape[10]), float.Parse(shape[12]),
                        float.Parse(shape[4]), float.Parse(shape[6]), int.Parse(shape[2]), Color.FromName(shape[14]).Name);
                    Shapes.Add(triangle);
                }
            }
        }
    }
}

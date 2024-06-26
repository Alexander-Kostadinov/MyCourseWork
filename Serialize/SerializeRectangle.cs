﻿using Shapes;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Serialization
{
    public class SerializeRectangle : Serializer
    {
        public SerializeRectangle(List<IDrawable> shapes, string text) : base(shapes, text) { }

        public override string Serialize()
        {
            var rectangles = Shapes.Where(x => x.GetType().Name == "Rectangle").ToArray();

            foreach (var rectangle in rectangles)
            {
                var type = rectangle.GetType();
                var a = type.GetProperty("A").GetValue(rectangle);
                var b = type.GetProperty("B").GetValue(rectangle);

                text += $"Rectangle Id: {rectangle.ID} X: {rectangle.X} Y: {rectangle.Y} " +
                    $"A: {a} B: {b}  Color: {rectangle.Color}\n";
            }

            return text;
        }

        public override void Deserialize()
        {
            var lines = Text.Split('\n').ToList();
            lines.RemoveAt(lines.Count() - 1);

            foreach (var line in lines)
            {
                var shape = line.Split(' ').ToArray();

                if (shape[0] == "Rectangle")
                {
                    Shapes.Rectangle rectangle = new Shapes.Rectangle(float.Parse(shape[8]), 
                        float.Parse(shape[10]), float.Parse(shape[4]), float.Parse(shape[6]), 
                        int.Parse(shape[2]), Color.FromName(shape[13]).Name);

                    Shapes.Add(rectangle);
                }
            }
        }

    }
}

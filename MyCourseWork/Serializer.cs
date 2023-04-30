using System;
using Shapes;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MyCourseWork
{
    public class Serializer
    {
        public void Serialize(List<IDrawable> Shapes)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "*.txt|";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string docPath = Directory.GetCurrentDirectory();

                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, saveFile.FileName)))
                    {
                        foreach (var shape in Shapes)
                        {
                            var type = shape.GetType().Name;

                            switch (type)
                            {
                                case "Circle":
                                    outputFile.WriteLine($"{type} Id: {shape.ID} " +
                                        $"X: {shape.X} Y: {shape.Y} Radius: {shape.FirstSide} Color: {shape.Color}");
                                    break;

                                case "Rectangle":
                                    outputFile.WriteLine($"{type} Id: {shape.ID} X: {shape.X} Y: {shape.Y} " +
                                        $"A: {shape.FirstSide} B: {shape.SecondSide}  Color: {shape.Color}");
                                    break;

                                case "Triangle":
                                    outputFile.WriteLine($"{type} Id: {shape.ID} X: {shape.X} Y: {shape.Y} " +
                                        $"A: {shape.FirstSide} B: {shape.SecondSide} C: {shape.ThirdSide}" +
                                        $" Color: {shape.Color}");
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Directory not found.");
                }
                catch
                {
                    MessageBox.Show("Error!");
                }
            }
        }
        public void Deserialize(List<IDrawable> Shapes, List<Command> UndoCommands, List<Command> RedoCommands, int ID)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.txt|";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFile.FileName);

                    List<int> ids = new List<int>();

                    while (true)
                    {
                        var line = sr.ReadLine();

                        if (line == null)
                        {
                            break;
                        }

                        var type = line.Split(' ').ToArray();

                        ids = Shapes.Select(x => x.ID).
                            Concat(UndoCommands.Select(x => x.Item.ID)).
                            Concat(RedoCommands.Select(x => x.Item.ID)).ToList();

                        if (ids != null && ids.Contains(int.Parse(type[2])))
                        {
                            ID = ids.OrderBy(x => x).Reverse().First();
                            ID++;

                            type[2] = ID.ToString();
                        }
                        else if (ids != null && ID <= int.Parse(type[2]))
                        {
                            ID++;
                            type[2] = ID.ToString();
                        }

                        switch (type[0])
                        {
                            case "Circle":
                                var circle = new Circle(float.Parse(type[8]), float.Parse(type[4]),
                                    float.Parse(type[6]), int.Parse(type[2]), Color.FromName(type[10]).Name);
                                Shapes.Add(circle);
                                break;

                            case "Rectangle":
                                var rectangle = new Shapes.Rectangle(float.Parse(type[8]), float.Parse(type[10]),
                                    float.Parse(type[4]), float.Parse(type[6]), int.Parse(type[2]), Color.FromName(type[13]).Name);
                                Shapes.Add(rectangle);
                                break;

                            case "Triangle":
                                var triangle = new Triangle(float.Parse(type[8]), float.Parse(type[10]), float.Parse(type[12]),
                                    float.Parse(type[4]), float.Parse(type[6]), int.Parse(type[2]), Color.FromName(type[14]).Name);
                                Shapes.Add(triangle);
                                break;

                            default:
                                break;
                        }
                    }

                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}

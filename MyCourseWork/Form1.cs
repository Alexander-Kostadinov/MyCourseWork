using Shapes;
using System;
using System.IO;
using System.Linq;
using System.Data;
using Serialization;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MyCourseWork
{
    public partial class Form1 : Form
    {
        private Add Add { get; set; }
        private Fill Fill { get; set; }
        private Clear Clear { get; set; }
        private Moving Moving { get; set; }
        private Remove Remove { get; set; }

        Color Color { get; set; }
        private int ID { get; set; }
        private Pen Pen { get; set; }
        private int CurrentID { get; set; }
        private string ShapeType { get; set; }
        private List<IDrawable> Shapes { get; set; }
        private List<ICommand> UndoCommands { get; set; }
        private List<ICommand> RedoCommands { get; set; }
        private List<ISerializable> Serializables { get; set; }

        public Form1()
        {
            InitializeComponent();

            ID = 0;
            CurrentID = 0;
            ShapeType = string.Empty;
            Color = Color.Transparent;
            Pen = new Pen(Color.Black, 3);
            Shapes = new List<IDrawable>();
            UndoCommands = new List<ICommand>();
            RedoCommands = new List<ICommand>();
            Serializables = new List<ISerializable>();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    Color = Color.Transparent;

                    if (circleToolStripMenuItem.Checked == false && triangleToolStripMenuItem.Checked == false && rectangleToolStripMenuItem.Checked == false)
                    {
                        return;
                    }

                    var IDs = Shapes.Select(x => x.ID).ToList();

                    if (IDs.Contains(ID + 1))
                    {
                        ID = Shapes.Select(x => x.ID).OrderBy(x => x).LastOrDefault();
                    }

                    switch (ShapeType)
                    {
                        case "Circle":
                            ID++;
                            var circle = new Circle(float.Parse(toolStripTextBox2.Text), e.X, e.Y, ID, Color.ToArgb().ToString());
                            Add = new Add(circle, Shapes);
                            Add.Execute();
                            UndoCommands.Add(Add);
                            RedoCommands.Clear();
                            break;

                        case "Triangle":
                            ID++;
                            var triangle = new Triangle(float.Parse(toolStripTextBox4.Text),
                                float.Parse(toolStripTextBox6.Text), 
                                float.Parse(toolStripTextBox8.Text), e.X, e.Y, ID, Color.ToArgb().ToString());
                            Add = new Add(triangle, Shapes);
                            Add.Execute();
                            UndoCommands.Add(Add);
                            RedoCommands.Clear();
                            break;

                        case "Rectangle":
                            ID++;
                            var rectangle = new Shapes.Rectangle(float.Parse(toolStripTextBox10.Text),
                                float.Parse(toolStripTextBox12.Text), e.X, e.Y, ID, Color.ToArgb().ToString());
                            Add = new Add(rectangle, Shapes);
                            Add.Execute();
                            UndoCommands.Add(Add);
                            RedoCommands.Clear();
                            break;
                    }

                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var point = new Shapes.Point(e.Location.X, e.Location.Y);

                var selected = Shapes.Where(x => x.Contains(point)).LastOrDefault();

                if (selected == null)
                {
                    return;
                }

                var unselect = Shapes.Where(x => x.ID < 0).FirstOrDefault();

                if (unselect != null && unselect.ID != selected.ID)
                {
                    unselect.ID *= -1;
                }

                selected.ID *= -1;

                Invalidate();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var point = new Shapes.Point(e.Location.X, e.Location.Y);
                var shape = Shapes.Where(x => x.Contains(point)).LastOrDefault();

                if (shape != null)
                {
                    Moving = new Moving(shape, Shapes, (int)shape.X, (int)shape.Y);
                    CurrentID = shape.ID;
                    shape.ID = 0;
                }

                SuspendLayout();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var shape = Shapes.Where(x => x.ID == 0).FirstOrDefault();

                if (shape != null)
                {
                    var type = shape.GetType();

                    switch (type.Name)
                    {
                        case "Circle":
                            var r = type.GetField("radius", BindingFlags.Instance | BindingFlags.NonPublic).
                                GetValue(shape).ToString();

                            shape.X = e.X - (int.Parse(r) / 2);
                            shape.Y = e.Y - (int.Parse(r) / 2);
                            break;

                        case "Rectangle":
                            var recA = type.GetField("a", BindingFlags.Instance | BindingFlags.NonPublic).
                                GetValue(shape).ToString();
                            var recB = type.GetField("b", BindingFlags.Instance | BindingFlags.NonPublic).
                                GetValue(shape).ToString();

                            shape.X = e.X - (int.Parse(recA) / 2);
                            shape.Y = e.Y - (int.Parse(recB) / 2);
                            break;

                        case "Triangle":
                            var a = type.GetField("a", BindingFlags.Instance | BindingFlags.NonPublic).
                                GetValue(shape).ToString();
                            var b = type.GetField("b", BindingFlags.Instance | BindingFlags.NonPublic).
                                GetValue(shape).ToString();
                            var c = type.GetField("c", BindingFlags.Instance | BindingFlags.NonPublic).
                                GetValue(shape).ToString();

                            var h = 2 * shape.Surface / int.Parse(c);

                            if (int.Parse(a) > int.Parse(b) && int.Parse(a) > int.Parse(c))
                            {
                                var distance = Math.Sqrt((int.Parse(b) * int.Parse(b)) - (h * h));

                                shape.X = e.X - (int)distance;
                            }
                            else if (int.Parse(b) > int.Parse(a) && int.Parse(b) > int.Parse(c))
                            {
                                var distance = Math.Sqrt((int.Parse(a) * int.Parse(a)) - (h * h));

                                shape.X = e.X + (int)distance;
                            }
                            else
                            {
                                shape.X = e.X;
                            }

                            shape.Y = (int)(e.Y - ((float)h / 2));
                            break;
                    }

                    Invalidate();
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var shape = Shapes.Where(x => x.ID == 0).FirstOrDefault();

                if (shape == null)
                {
                    return;
                }

                shape.ID = CurrentID;
                CurrentID = 0;

                if (shape.X == Moving.X && shape.Y == Moving.Y)
                {
                    return;
                }

                var type = shape.GetType();

                switch (type.Name)
                {
                    case "Circle":
                        var r = type.GetField("radius", BindingFlags.Instance | BindingFlags.NonPublic).
                            GetValue(shape).ToString();

                        Moving.X = e.X - (int.Parse(r) / 2);
                        Moving.Y = e.Y - (int.Parse(r) / 2);
                        break;

                    case "Rectangle":
                        var recA = type.GetField("a", BindingFlags.Instance | BindingFlags.NonPublic).
                            GetValue(shape).ToString();
                        var recB = type.GetField("b", BindingFlags.Instance | BindingFlags.NonPublic).
                            GetValue(shape).ToString();

                        Moving.X = e.X - (int.Parse(recA) / 2);
                        Moving.Y = e.Y - (int.Parse(recB) / 2);
                        break;

                    case "Triangle":
                        var a = type.GetField("a", BindingFlags.Instance | BindingFlags.NonPublic).
                            GetValue(shape).ToString();
                        var b = type.GetField("b", BindingFlags.Instance | BindingFlags.NonPublic).
                            GetValue(shape).ToString();
                        var c = type.GetField("c", BindingFlags.Instance | BindingFlags.NonPublic).
                            GetValue(shape).ToString();

                        var h = 2 * shape.Surface / int.Parse(c);

                        if (int.Parse(a) > int.Parse(b) && int.Parse(a) > int.Parse(c))
                        {
                            var distance = Math.Sqrt((int.Parse(b) * int.Parse(b)) - (h * h));

                            Moving.X = e.X - (int)distance;
                        }
                        else if (int.Parse(b) > int.Parse(a) && int.Parse(b) > int.Parse(c))
                        {
                            var distance = Math.Sqrt((int.Parse(a) * int.Parse(a)) - (h * h));

                            Moving.X = e.X + (int)distance;
                        }
                        else
                        {
                            Moving.X = e.X;
                        }

                        Moving.Y = (int)(e.Y - ((float)h / 2));
                        break;
                }

                Moving.Execute();
                UndoCommands.Add(Moving);
                RedoCommands.Clear();
                ResumeLayout();
                Invalidate();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            foreach (var shape in Shapes)
            {
                if (shape.ID < 0)
                {
                    Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }

                Color = ColorTranslator.FromHtml(shape.Color);

                var type = shape.GetType().Name;

                switch (type)
                {
                    case "Circle":
                        var pointsCir = shape.GetPoints(shape.X, shape.Y);
                        var pointsCirF = new PointF[4];
                        pointsCirF[0] = new PointF(pointsCir[0].X, pointsCir[0].Y);
                        pointsCirF[1] = new PointF(pointsCir[1].X, pointsCir[1].Y);
                        pointsCirF[2] = new PointF(pointsCir[2].X, pointsCir[2].Y);
                        pointsCirF[3] = new PointF(pointsCir[3].X, pointsCir[3].Y);

                        var width = pointsCir[1].X - shape.X;
                        var height = pointsCir[3].Y - shape.Y;

                        e.Graphics.DrawEllipse(Pen, shape.X, shape.Y, width, height);
                        e.Graphics.FillEllipse(new SolidBrush(Color), shape.X, shape.Y, width, height);

                        break;

                    case "Rectangle":
                        var pointsRec = shape.GetPoints(shape.X, shape.Y);
                        var pointsRecF = new PointF[4];
                        pointsRecF[0] = new PointF(pointsRec[0].X, pointsRec[0].Y);
                        pointsRecF[1] = new PointF(pointsRec[1].X, pointsRec[1].Y);
                        pointsRecF[2] = new PointF(pointsRec[2].X, pointsRec[2].Y);
                        pointsRecF[3] = new PointF(pointsRec[3].X, pointsRec[3].Y);

                        e.Graphics.DrawPolygon(Pen, pointsRecF);
                        e.Graphics.FillPolygon(new SolidBrush(Color), pointsRecF);
                        break;

                    case "Triangle":
                        var points = shape.GetPoints(shape.X, shape.Y);
                        var pointsF = new PointF[3];
                        pointsF[0] = new PointF(points[0].X, points[0].Y);
                        pointsF[1] = new PointF(points[1].X, points[1].Y);
                        pointsF[2] = new PointF(points[2].X, points[2].Y);

                        e.Graphics.DrawPolygon(Pen, pointsF);
                        e.Graphics.FillPolygon(new SolidBrush(Color), pointsF);
                        break;
                }

                Pen = new Pen(Color.Black, 3);
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "*.txt|";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader reader = new StreamReader(openFile.FileName);

                    var text = reader.ReadToEnd();

                    var serialized = new List<IDrawable>();
                    var circles = new SerializeCircle(serialized, text);
                    var triangles = new SerializeTriangle(serialized, text);
                    var rectangles = new SerializeRectangle(serialized, text);

                    Serializables.Add(circles);
                    Serializables.Add(triangles);
                    Serializables.Add(rectangles);

                    foreach (var shape in Serializables)
                    {
                        shape.Deserialize();
                    }

                    var ids = Shapes.Select(x => x.ID).ToList();

                    foreach (var shape in serialized)
                    {
                        if (ids.Contains(shape.ID))
                        {
                            shape.ID = ids.Max() + 1;
                            ids.Add(shape.ID);
                            ID = shape.ID;
                        }

                        ids.Add(shape.ID);
                        Shapes.Add(shape);
                    }

                    ids.Clear();
                    serialized.Clear();
                    Serializables.Clear();

                    reader.Close();

                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
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
                        var circles = new SerializeCircle(Shapes, string.Empty);
                        var triangles = new SerializeTriangle(Shapes, string.Empty);
                        var rectangles = new SerializeRectangle(Shapes, string.Empty);

                        Serializables.Add(circles);
                        Serializables.Add(triangles);
                        Serializables.Add(rectangles);

                        var dictionary = new Dictionary<int, string>();

                        foreach (var serializable in Serializables)
                        {
                            var shapes = serializable.Serialize().Split('\n').ToList();
                            shapes.RemoveAt(shapes.Count() - 1);

                            foreach (var shape in shapes)
                            {
                                dictionary.Add(int.Parse(shape.Split(' ')[2]), shape);
                            }
                        }

                        var text = dictionary.OrderBy(x => x.Key).Select(s => s.Value).ToList();

                        foreach (var line in text)
                        {
                            outputFile.WriteLine(line);
                        }

                        text.Clear();
                        dictionary.Clear();
                        Serializables.Clear();
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

        private void ToolStripMenu_Color(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                var selected = Shapes.Where(x => x.ID < 0).LastOrDefault();

                if (selected != null && selected.Color != colorDialog1.Color.ToArgb().ToString())
                {
                    Fill = new Fill(selected, Shapes, colorDialog1.Color);
                    Fill.Execute();
                    UndoCommands.Add(Fill);
                    RedoCommands.Clear();
                    Invalidate();
                }
            }
        }

        private void ToolStripMenu_Undo(object sender, EventArgs e)
        {
            var command = UndoCommands.LastOrDefault();

            if (command == null)
            {
                return;
            }

            command.UndoExecute();

            RedoCommands.Add(command);
            UndoCommands.Remove(command);
            Invalidate();
        }

        private void ToolStripMenu_Redo(object sender, EventArgs e)
        {
            var command = RedoCommands.LastOrDefault();

            if (command == null)
            {
                return;
            }

            command.Execute();

            UndoCommands.Add(command);
            RedoCommands.Remove(command);
            Invalidate();
        }

        private void ToolStripMenu_Remove(object sender, EventArgs e)
        {
            var selected = Shapes.Where(x => x.ID < 0).LastOrDefault();

            if (selected == null)
            {
                return;
            }

            selected.ID *= -1;
            Remove = new Remove(selected, Shapes);
            Remove.Execute();
            UndoCommands.Add(Remove);
            RedoCommands.Clear();
            Invalidate();
        }

        private void ToolStripMenu_Clear(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete all items ?",
                                     "Confirm Clear !", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                Clear = new Clear(null, Shapes, UndoCommands, RedoCommands);
                Clear.Execute();
                toolStripTextBox2.Text = string.Empty;
                toolStripTextBox4.Text = string.Empty;
                toolStripTextBox6.Text = string.Empty;
                toolStripTextBox8.Text = string.Empty;
                toolStripTextBox10.Text = string.Empty;
                toolStripTextBox12.Text = string.Empty;
                circleToolStripMenuItem.Checked = false;
                triangleToolStripMenuItem.Checked = false;
                rectangleToolStripMenuItem.Checked = false;
                CreateGraphics().Clear(DefaultBackColor);
                Refresh();
            }
        }

        private void CircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (circleToolStripMenuItem.Checked == true)
            {
                triangleToolStripMenuItem.Checked = false;
                rectangleToolStripMenuItem.Checked = false;

                ShapeType = "Circle";
            }
        }

        private void TriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (triangleToolStripMenuItem.Checked == true)
            {
                circleToolStripMenuItem.Checked = false;
                rectangleToolStripMenuItem.Checked = false;

                ShapeType = "Triangle";
            }
        }

        private void RectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rectangleToolStripMenuItem.Checked == true)
            {
                circleToolStripMenuItem.Checked = false;
                triangleToolStripMenuItem.Checked = false;

                ShapeType = "Rectangle";
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            var selected = Shapes.Where(x => x.ID < 0).FirstOrDefault();

            if (selected == null)
            {
                return;
            }

            var type = selected.GetType();

            switch (type.Name)
            {
                case "Circle":
                    MessageBox.Show($" {selected.GetType().Name}\n Radius: {type.GetProperty("Radius").GetValue(selected)}\n " +
                        $"Surface: {selected.Surface}\n Perimeter: {selected.Perimeter}");
                    break;

                case "Triangle":
                    MessageBox.Show($" {selected.GetType().Name}\n A: {type.GetProperty("A").GetValue(selected)}\n " +
                        $"B: {type.GetProperty("B").GetValue(selected)}\n C: {type.GetProperty("C").GetValue(selected)}\n " +
                        $"Surface: {selected.Surface}\n Perimeter: {selected.Perimeter}");
                    break;

                case "Rectangle":
                    MessageBox.Show($" {selected.GetType().Name}\n A: {type.GetProperty("A").GetValue(selected)}\n " +
                        $"B: {type.GetProperty("B").GetValue(selected)}\n Surface: {selected.Surface}\n " +
                        $"Perimeter: {selected.Perimeter}");
                    break;
            }
        }
    }
}

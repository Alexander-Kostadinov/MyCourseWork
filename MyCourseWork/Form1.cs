using Shapes;
using System;
using System.IO;
using System.Linq;
using System.Data;
using Serialization;
using System.Drawing;
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

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            var selected = Shapes.Where(x => x.ID < 0).LastOrDefault();

            if (selected != null && selected.Color != flowLayoutPanel1.BackColor.ToArgb().ToString())
            {
                Fill = new Fill(selected, Shapes, flowLayoutPanel1.BackColor);
                Fill.Execute();
                UndoCommands.Add(Fill);
                RedoCommands.Clear();
                Invalidate();
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    ID = Shapes.Select(x => x.ID).OrderBy(x => x).LastOrDefault();

                    switch (ShapeType)
                    {
                        case "Circle":
                            ID++;
                            var circle = new Circle(float.Parse(textBox1.Text), e.X, e.Y, ID, Color.ToArgb().ToString());
                            Add = new Add(circle, Shapes);
                            Add.Execute();
                            UndoCommands.Add(Add);
                            RedoCommands.Clear();
                            break;

                        case "Triangle":
                            ID++;
                            var triangle = new Triangle(float.Parse(textBox4.Text),
                                float.Parse(textBox6.Text), float.Parse(textBox5.Text), e.X, e.Y, ID, Color.ToArgb().ToString());
                            Add = new Add(triangle, Shapes);
                            Add.Execute();
                            UndoCommands.Add(Add);
                            RedoCommands.Clear();
                            break;

                        case "Rectangle":
                            ID++;
                            var rectangle = new Shapes.Rectangle(float.Parse(textBox2.Text),
                                float.Parse(textBox3.Text), e.X, e.Y, ID, Color.ToArgb().ToString());
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

                var type = selected.GetType().Name;

                switch (type)
                {
                    case "Circle":
                        textBox1.Text = selected.FirstSide.ToString();
                        break;
                    case "Rectangle":
                        textBox2.Text = selected.FirstSide.ToString();
                        textBox3.Text = selected.SecondSide.ToString();
                        break;
                    case "Triangle":
                        textBox4.Text = selected.FirstSide.ToString();
                        textBox6.Text = selected.SecondSide.ToString();
                        textBox5.Text = selected.ThirdSide.ToString();
                        break;
                    default:
                        break;
                }

                textBox7.Text = selected.Perimeter.ToString();
                textBox8.Text = selected.Surface.ToString();

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
                    var type = shape.GetType().Name;

                    switch (type)
                    {
                        case "Circle":
                            shape.X = e.X - (shape.FirstSide / 2);
                            shape.Y = e.Y - (shape.FirstSide / 2);
                            break;
                        case "Rectangle":
                            shape.X = e.X - (shape.FirstSide / 2);
                            shape.Y = e.Y - (shape.SecondSide / 2);
                            break;
                        case "Triangle":
                            var h = 2 * shape.Surface / shape.ThirdSide;
                            if (shape.FirstSide > shape.SecondSide && shape.FirstSide > shape.ThirdSide)
                            {
                                shape.X = e.X - (shape.ThirdSide / 2);
                            }
                            else if (shape.SecondSide > shape.FirstSide && shape.SecondSide > shape.ThirdSide)
                            {
                                shape.X = e.X + (shape.ThirdSide / 2);
                            }
                            else
                            {
                                shape.X = e.X;
                            }
                            shape.Y = e.Y - ((float)h / 2);
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

                var type = shape.GetType().Name;

                switch (type)
                {
                    case "Circle":
                        Moving.X = e.X - (int)(shape.FirstSide / 2);
                        Moving.Y = e.Y - (int)(shape.FirstSide / 2);
                        break;
                    case "Rectangle":
                        Moving.X = e.X - (int)(shape.FirstSide / 2);
                        Moving.Y = e.Y - (int)(shape.SecondSide / 2);
                        break;
                    case "Triangle":
                        var h = 2 * shape.Surface / shape.ThirdSide;
                        if (shape.FirstSide > shape.SecondSide && shape.FirstSide > shape.ThirdSide)
                        {
                            Moving.X = e.X - (int)(shape.ThirdSide / 2);
                        }
                        else if (shape.SecondSide > shape.FirstSide && shape.SecondSide > shape.ThirdSide)
                        {
                            Moving.X = e.X + (int)(shape.ThirdSide / 2);
                        }
                        else
                        {
                            Moving.X = e.X;
                        }
                        Moving.Y = e.Y - (int)((float)h / 2);
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
            var moving = Shapes.Where(x => x.ID == 0).FirstOrDefault();
            var isMoving = Shapes.Contains(moving);

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
                        e.Graphics.DrawEllipse(Pen, shape.X, shape.Y, shape.FirstSide, shape.FirstSide);
                        e.Graphics.FillEllipse(new SolidBrush(Color), shape.X, shape.Y, shape.FirstSide, shape.FirstSide);
                        break;

                    case "Rectangle":
                        e.Graphics.DrawRectangle(Pen, shape.X, shape.Y, shape.FirstSide, shape.SecondSide);
                        e.Graphics.FillRectangle(new SolidBrush(Color), shape.X, shape.Y, shape.FirstSide, shape.SecondSide);
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

        private void Button_Save(object sender, EventArgs e)
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

                        foreach (var shape in Serializables)
                        {
                            outputFile.Write(shape.Serialize());
                        }

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

        private void Button_Open(object sender, EventArgs e)
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

        private void Button_Redo(object sender, EventArgs e)
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

        private void Button_Undo(object sender, EventArgs e)
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

        private void Button_Clear(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete all items ?",
                                     "Confirm Clear !", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                Clear = new Clear(null, Shapes, UndoCommands, RedoCommands, Serializables);
                Clear.Execute();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox6.Text = string.Empty;
                textBox7.Text = string.Empty;
                textBox8.Text = string.Empty;
                flowLayoutPanel1.BackColor = Color.Transparent;
                CreateGraphics().Clear(DefaultBackColor);
                Refresh();
            }
        }

        private void Button_Remove(object sender, EventArgs e)
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

        private void Button_Colors(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                flowLayoutPanel1.BackColor = colorDialog1.Color;
                flowLayoutPanel1.Invalidate();
            }
        }

        private void RadioButton_Circle(object sender, EventArgs e)
        {
            ShapeType = "Circle";
        }

        private void RadioButton_Triangle(object sender, EventArgs e)
        {
            ShapeType = "Triangle";
        }

        private void RadioButton_Rectangle(object sender, EventArgs e)
        {
            ShapeType = "Rectangle";
        }
    }
}

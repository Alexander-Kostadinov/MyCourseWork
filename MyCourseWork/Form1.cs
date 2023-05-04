using Shapes;
using System;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

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
            Pen = new Pen(Color.Black, 2);
            Shapes = new List<IDrawable>();
            UndoCommands = new List<ICommand>();
            RedoCommands = new List<ICommand>();
            Serializables = new List<ISerializable>();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e, float x, float y)
        {
            y += panel1.Location.Y;
            Color = Color.Transparent;

            try
            {
                switch (ShapeType)
                {
                    case "Circle":
                        ID++;
                        var circle = new Circle(float.Parse(textBox1.Text), x, y, ID, Color.ToArgb().ToString());
                        Add = new Add(circle, Shapes);
                        Add.Execute();
                        UndoCommands.Add(Add);
                        RedoCommands.Clear();
                        break;

                    case "Triangle":
                        ID++;
                        var triangle = new Triangle(float.Parse(textBox4.Text),
                            float.Parse(textBox6.Text), float.Parse(textBox5.Text), x, y, ID, Color.ToArgb().ToString());
                        Add = new Add(triangle, Shapes);
                        Add.Execute();
                        UndoCommands.Add(Add);
                        RedoCommands.Clear();
                        break;

                    case "Rectangle":
                        ID++;
                        var rectangle = new Shapes.Rectangle(float.Parse(textBox2.Text),
                            float.Parse(textBox3.Text), x, y, ID, Color.ToArgb().ToString());
                        Add = new Add(rectangle, Shapes);
                        Add.Execute();
                        UndoCommands.Add(Add);
                        RedoCommands.Clear();
                        break;

                    default:
                        break;
                }

                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            var selected = Shapes.Where(x => x.ID < 0).LastOrDefault();

            if (selected == null)
            {
                return;
            }

            if (selected.Color == flowLayoutPanel1.BackColor.ToArgb().ToString())
            {
                return;
            }

            Fill = new Fill(selected, Shapes, flowLayoutPanel1.BackColor);
            Fill.Execute();
            UndoCommands.Add(Fill);
            RedoCommands.Clear();
            Refresh();
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            switch (e.Button)
            {
                case MouseButtons.Left:

                    Panel1_Paint(sender,
                new PaintEventArgs(panel1.CreateGraphics(),
                new System.Drawing.Rectangle(new System.Drawing.Point(panel1.Location.X, panel1.Location.Y),
                new Size(panel1.Width, panel1.Height))), e.Location.X, e.Location.Y);
                    break;

                default:
                    break;
            }

            Refresh();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);

            switch (e.Button)
            {
                case MouseButtons.Left:

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

                    Refresh();
                    break;

                default:
                    break;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            switch (e.Button)
            {
                case MouseButtons.Right:

                    var point = new Shapes.Point(e.Location.X, e.Location.Y);
                    var shape = Shapes.Where(x => x.Contains(point)).LastOrDefault();

                    if (shape == null)
                    {
                        return;
                    }

                    Moving = new Moving(shape, Shapes, (int)shape.X, (int)shape.Y);
                    CurrentID = shape.ID;
                    shape.ID = 0;
                    break;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Shapes.Count() == 0)
            {
                return;
            }

            var shape = Shapes.Where(x => x.ID == 0).FirstOrDefault();

            if (shape == null)
            {
                return;
            }

            if (e.Location.Y + 88 >= panel1.Location.Y)
            {
                shape.X = e.Location.X;
                shape.Y = e.Location.Y + 88;
            }

            Refresh();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);

            switch (e.Button)
            {
                case MouseButtons.Right:
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

                    Moving.X = e.X;
                    Moving.Y = e.Y + 88;
                    Moving.Execute();
                    UndoCommands.Add(Moving);
                    RedoCommands.Clear();
                    Refresh();
                    break;

                default:
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var shape in Shapes)
            {
                Color = ColorTranslator.FromHtml(shape.Color);

                var type = shape.GetType().Name;

                switch (type)
                {
                    case "Circle":
                        if (shape.ID < 0)
                        {
                            Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        }
                        e.Graphics.DrawEllipse(Pen, shape.X, shape.Y, shape.FirstSide, shape.FirstSide);
                        e.Graphics.FillEllipse(new SolidBrush(Color), shape.X, shape.Y, shape.FirstSide, shape.FirstSide);
                        break;

                    case "Rectangle":
                        if (shape.ID < 0)
                        {
                            Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        }
                        e.Graphics.DrawRectangle(Pen, shape.X, shape.Y, shape.FirstSide, shape.SecondSide);
                        e.Graphics.FillRectangle(new SolidBrush(Color), shape.X, shape.Y, shape.FirstSide, shape.SecondSide);
                        break;

                    case "Triangle":
                        if (shape.ID < 0)
                        {
                            Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        }

                        var points = shape.GetPoints(shape.X, shape.Y);
                        var pointsF = new PointF[3];
                        pointsF[0] = new PointF(points[0].X, points[0].Y);
                        pointsF[1] = new PointF(points[1].X, points[1].Y);
                        pointsF[2] = new PointF(points[2].X, points[2].Y);

                        e.Graphics.DrawPolygon(Pen, pointsF);
                        e.Graphics.FillPolygon(new SolidBrush(Color), pointsF);
                        break;

                    default:
                        break;
                }

                Pen = new Pen(Color.Black, 2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                panel1.CreateGraphics().Clear(DefaultBackColor);

                Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var command = RedoCommands.LastOrDefault();

            if (command == null)
            {
                return;
            }

            command.Execute();

            UndoCommands.Add(command);
            RedoCommands.Remove(command);
            Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var command = UndoCommands.LastOrDefault();

            if (command == null)
            {
                return;
            }

            command.UndoExecute();

            RedoCommands.Add(command);
            UndoCommands.Remove(command);
            Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
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
            Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                flowLayoutPanel1.BackColor = colorDialog1.Color;
                Refresh();
            }
        }

        private void button6_Click(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.txt|";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader reader = new StreamReader(openFile.FileName);

                    var text = reader.ReadToEnd();

                    var circles = new SerializeCircle(Shapes, text);
                    var triangles = new SerializeTriangle(Shapes, text);
                    var rectangles = new SerializeRectangle(Shapes, text);

                    Serializables.Add(circles);
                    Serializables.Add(triangles);
                    Serializables.Add(rectangles);

                    foreach (var shape in Serializables)
                    {
                        shape.Deserialize();
                    }

                    Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShapeType = "Circle";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ShapeType = "Triangle";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ShapeType = "Rectangle";
        }
    }
}

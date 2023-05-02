using Shapes;
using System;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MyCourseWork
{
    public partial class Form1 : Form
    {
        Color Color { get; set; }
        public int ID { get; set; }
        public Pen Pen { get; set; }
        public int CurrentID { get; set; }
        public string ShapeType { get; set; }
        public List<IDrawable> Shapes { get; set; }

        public Serializer Serializer { get; set; }

        public Command Command { get; set; }
        public List<Command> UndoCommands { get; set; }
        public List<Command> RedoCommands { get; set; }

        Undo Undo { get; set; }
        Redo Redo { get; set; }
        Clear Clear { get; set; }
        Remove Remove { get; set; }

        public Form1()
        {
            InitializeComponent();

            ID = 0;
            CurrentID = 0;
            ShapeType = string.Empty;
            Color = Color.Transparent;
            Pen = new Pen(Color.Black, 2);
            Shapes = new List<IDrawable>();

            Command = new Command();
            Serializer = new Serializer();
            UndoCommands = new List<Command>();
            RedoCommands = new List<Command>();
            Undo = new Undo(UndoCommands, RedoCommands, Shapes);
            Redo = new Redo(UndoCommands, RedoCommands, Shapes);
            Clear = new Clear(UndoCommands, RedoCommands, Shapes);
            Remove = new Remove(UndoCommands, RedoCommands, Shapes);
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
                        Shapes.Add(circle);
                        RedoCommands.Clear();
                        Command.Name = "Add";
                        Command.Item = circle;
                        UndoCommands.Add(Command);
                        textBox7.Text = circle.Perimeter.ToString();
                        textBox8.Text = circle.Surface.ToString();
                        break;

                    case "Triangle":
                        ID++;
                        var triangle = new Triangle(float.Parse(textBox4.Text),
                            float.Parse(textBox6.Text), float.Parse(textBox5.Text), x, y, ID, Color.ToArgb().ToString());
                        Shapes.Add(triangle);
                        RedoCommands.Clear();
                        Command.Name = "Add";
                        Command.Item = triangle;
                        UndoCommands.Add(Command);
                        textBox7.Text = triangle.Perimeter.ToString();
                        textBox8.Text = triangle.Surface.ToString();
                        break;

                    case "Rectangle":
                        ID++;
                        var rectangle = new Shapes.Rectangle(float.Parse(textBox2.Text),
                            float.Parse(textBox3.Text), x, y, ID, Color.ToArgb().ToString());
                        Shapes.Add(rectangle);
                        RedoCommands.Clear();
                        Command.Name = "Add";
                        Command.Item = rectangle;
                        UndoCommands.Add(Command);
                        textBox7.Text = rectangle.Perimeter.ToString();
                        textBox8.Text = rectangle.Surface.ToString();
                        break;

                    default:
                        break;
                }

                Command = new Command();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Refresh();
        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            var selected = Shapes.Where(x => x.ID < 0).FirstOrDefault();

            if (selected == null)
            {
                return;
            }

            Command.Name = "Fill";
            Command.Color = Color.FromName(selected.Color);
            Color = flowLayoutPanel1.BackColor;
            selected.Color = Color.ToArgb().ToString();

            var type = selected.GetType().Name;

            switch (type)
            {
                case "Circle":
                    var cloneCircle = new Circle(selected.FirstSide,
                        selected.X, selected.Y, selected.ID, selected.Color);
                    Command.Item = cloneCircle;
                    break;

                case "Rectangle":
                    var cloneRectangle = new Shapes.Rectangle(selected.FirstSide,
                        selected.SecondSide, selected.X, selected.Y, selected.ID, selected.Color);
                    Command.Item = cloneRectangle;
                    break;

                case "Triangle":
                    var cloneTriangle = new Triangle(selected.FirstSide, selected.SecondSide,
                        selected.ThirdSide, selected.X, selected.Y, selected.ID, selected.Color);
                    Command.Item = cloneTriangle;
                    break;

                default:
                    break;
            }

            RedoCommands.Clear();
            UndoCommands.Add(Command);
            Command = new Command();

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

                    CurrentID = shape.ID;
                    shape.ID = 0;
                    Command.X = (int)shape.X;
                    Command.Y = (int)shape.Y;
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

                    var type = shape.GetType().Name;

                    shape.ID = CurrentID;
                    CurrentID = 0;

                    if (shape.X == Command.X && shape.Y == Command.Y)
                    {
                        return;
                    }

                    switch (type)
                    {
                        case "Circle":
                            var cloneCircle = new Circle(shape.FirstSide, shape.X, shape.Y, shape.ID, shape.Color);
                            Command.Item = cloneCircle;
                            RedoCommands.Clear();
                            Command.Name = "Move";
                            UndoCommands.Add(Command);
                            break;

                        case "Rectangle":
                            var cloneRectangle = new Shapes.Rectangle(shape.FirstSide,
                                shape.SecondSide, shape.X, shape.Y, shape.ID, shape.Color);
                            Command.Item = cloneRectangle;
                            RedoCommands.Clear();
                            Command.Name = "Move";
                            UndoCommands.Add(Command);
                            break;

                        case "Triangle":
                            var cloneTriangle = new Triangle(shape.FirstSide,
                                shape.SecondSide, shape.ThirdSide, shape.X, shape.Y, shape.ID, shape.Color);
                            Command.Item = cloneTriangle;
                            RedoCommands.Clear();
                            Command.Name = "Move";
                            UndoCommands.Add(Command);
                            break;

                        default:
                            break;
                    }

                    Command = new Command();
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
            Redo.Execute();
            Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Undo.Execute();
            Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Remove.Execute();
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
            Serializer.Serialize(Shapes);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Serializer.Deserialize(Shapes, UndoCommands, RedoCommands, ID);

            if (Shapes.Count() > 0)
            {
                var nextId = Shapes.Select(x => x.ID).ToArray().OrderByDescending(x => x).FirstOrDefault();
                ID = nextId;
            }

            Refresh();
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

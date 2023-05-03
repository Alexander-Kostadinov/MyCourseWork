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
        private int ID { get; set; }
        private Pen Pen { get; set; }
        private string ShapeType { get; set; }
        private Serializer Serializer { get; set; }
        private List<IDrawable> Shapes { get; set; }
        private List<Command> UndoCommands { get; set; }
        private List<Command> RedoCommands { get; set; }

        private Add Add { get; set; }
        private Fill Fill { get; set; }
        private Undo Undo { get; set; }
        private Redo Redo { get; set; }
        private Clear Clear { get; set; }
        private Remove Remove { get; set; }
        private Moving Moving { get; set; }

        public Form1()
        {
            InitializeComponent();

            ID = 0;
            ShapeType = string.Empty;
            Color = Color.Transparent;
            Pen = new Pen(Color.Black, 2);
            Serializer = new Serializer();
            Shapes = new List<IDrawable>();
            UndoCommands = new List<Command>();
            RedoCommands = new List<Command>();

            Undo = new Undo(UndoCommands, RedoCommands, Shapes);
            Redo = new Redo(UndoCommands, RedoCommands, Shapes);
            Clear = new Clear(UndoCommands, RedoCommands, Shapes);
            Remove = new Remove(UndoCommands, RedoCommands, Shapes);
            Moving = new Moving(UndoCommands, RedoCommands, Shapes);
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
                        Add = new Add(UndoCommands, RedoCommands, Shapes, circle);
                        Add.Execute();
                        break;

                    case "Triangle":
                        ID++;
                        var triangle = new Triangle(float.Parse(textBox4.Text),
                            float.Parse(textBox6.Text), float.Parse(textBox5.Text), x, y, ID, Color.ToArgb().ToString());
                        Add = new Add(UndoCommands, RedoCommands, Shapes, triangle);
                        Add.Execute();
                        break;

                    case "Rectangle":
                        ID++;
                        var rectangle = new Shapes.Rectangle(float.Parse(textBox2.Text),
                            float.Parse(textBox3.Text), x, y, ID, Color.ToArgb().ToString());
                        Add = new Add(UndoCommands, RedoCommands, Shapes, rectangle);
                        Add.Execute();
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
            Fill = new Fill(UndoCommands, RedoCommands, Shapes, flowLayoutPanel1.BackColor);
            Fill.Execute();
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
                    Moving.PreExecute(point);
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
                    Moving.Execute();
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

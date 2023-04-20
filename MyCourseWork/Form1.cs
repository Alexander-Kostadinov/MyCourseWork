using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCourseWork
{
    public partial class Form1 : Form
    {
        public int ID { get; set; }
        public string ShapeType { get; set; }
        public Color Color { get; set; }
        public List<IDrawable> Shapes { get; set; }
        public List<IDrawable> MovedShapes { get; set; }
        public List<IDrawable> SelectedShapes { get; set; }
        public Command Command { get; set; }
        public List<Command> UndoCommands { get; set; }
        public List<Command> RedoCommands { get; set; }
        public Undo Undo { get; set; }
        public Redo Redo { get; set; }
        public Clear Clear { get; set; }
        public Remove Remove { get; set; }

        public Form1()
        {
            InitializeComponent();
            
            ID = 0;
            ShapeType = string.Empty;
            Color = Color.Transparent;

            Shapes = new List<IDrawable>();
            MovedShapes = new List<IDrawable>();
            SelectedShapes = new List<IDrawable>();

            Command = new Command();
            UndoCommands = new List<Command>();
            RedoCommands = new List<Command>();

            Undo = new Undo(UndoCommands, RedoCommands, Shapes);
            Redo = new Redo(UndoCommands, RedoCommands, Shapes);
            Clear = new Clear(UndoCommands, RedoCommands, Shapes);
            Remove = new Remove(UndoCommands, RedoCommands, Shapes, SelectedShapes);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e, float x, float y)
        {
            x += panel1.Location.X;
            y += panel1.Location.Y;
            ID++;

            try
            {
                switch (ShapeType)
                {
                    case "Circle":
                        var circle = new Circle(float.Parse(textBox1.Text), x, y, ID, Color);
                        Command.Item = circle;
                        Shapes.Add(circle);
                        textBox7.Text = circle.Perimeter.ToString();
                        textBox8.Text = circle.Surface.ToString();
                        break;
                    case "Triangle":
                        var triangle = new Triangle(float.Parse(textBox4.Text),
                            float.Parse(textBox6.Text), float.Parse(textBox5.Text), x, y, ID, Color);
                        Command.Item = triangle;
                        Shapes.Add(triangle);
                        textBox7.Text = triangle.Perimeter.ToString();
                        textBox8.Text = triangle.Surface.ToString();
                        break;
                    case "Rectangle":
                        var rectangle = new Rectangle(float.Parse(textBox2.Text), 
                            float.Parse(textBox3.Text), x, y, ID, Color);
                        Command.Item = rectangle;
                        Shapes.Add(rectangle);
                        textBox7.Text = rectangle.Perimeter.ToString();
                        textBox8.Text = rectangle.Surface.ToString();
                        break;
                }

                Command.Name = "Add";
                UndoCommands.Add(Command);
                Command = new Command();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Refresh();
        }

        private void StartUp(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);

            switch (e.Button)
            {
                case MouseButtons.Left:

                    RedoCommands.Clear();
                    Color = Color.Transparent;

                    Panel1_Paint(sender,
                new PaintEventArgs(panel1.CreateGraphics(),
                new System.Drawing.Rectangle(new Point(panel1.Location.X, panel1.Location.Y),
                new Size(panel1.Width, panel1.Height))), e.Location.X, e.Location.Y);

                    break;

                case MouseButtons.Right:

                    foreach (var shape in Shapes)
                    {
                        if(shape.Contains(e.Location))
                        {
                            SelectedShapes.Add(shape);
                        }
                    }

                break;

                default:
                    break;
            }

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var shape in Shapes)
            {
                shape.Draw(e.Graphics, shape.X, shape.Y);
                shape.Fill(e.Graphics, shape.X, shape.Y);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShapeType = "Circle";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ShapeType = "Rectangle";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ShapeType = "Triangle";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Color = Color.Red;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Color = Color.Green;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Color = Color.Blue;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Color = Color.Yellow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete all items ?",
                                     "Confirm Clear !", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                Clear.Execute();

                ID = 0;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox6.Text = string.Empty;
                textBox7.Text = string.Empty;
                textBox8.Text = string.Empty;

                SelectedShapes.Clear();
                panel1.CreateGraphics().Clear(DefaultBackColor);

                Refresh();
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            Redo.Execute();

            Refresh();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:

                    foreach (var shape in Shapes)
                    {
                        if (shape.Contains(e.Location))
                        {
                            MovedShapes.Add(shape);
                            Command.X = (int)shape.X;
                            Command.Y = (int)shape.Y;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:

                    foreach (var shape in Shapes)
                    {
                        if (MovedShapes.Count < 1)
                        {
                            return;
                        }

                        if (shape.ID == MovedShapes[MovedShapes.Count() - 1].ID)
                        {
                            shape.X = e.X + 1;
                            shape.Y = e.Y + 88;

                            var type = shape.GetType().Name;

                            switch (type)
                            {
                                case "Circle":
                                    var cloneCircle = new Circle(float.Parse(textBox1.Text),
                                        shape.X, shape.Y, shape.ID, shape.Color);
                                    Command.Item = cloneCircle;
                                    break;
                                case "Rectangle":
                                    var cloneRectangle = new Rectangle(float.Parse(textBox2.Text),
                                        float.Parse(textBox3.Text), shape.X, shape.Y, shape.ID, shape.Color);
                                    Command.Item = cloneRectangle;
                                    break;
                                case "Triangle":
                                    var cloneTriangle = new Triangle(float.Parse(textBox4.Text),
                                        float.Parse(textBox6.Text), float.Parse(textBox5.Text),
                                        shape.X, shape.Y, shape.ID, shape.Color);
                                    Command.Item = cloneTriangle;
                                    break;
                                default:
                                    break;
                            }

                            MovedShapes.Clear();

                            RedoCommands.Clear();

                            Command.Name = "Move";
                            UndoCommands.Add(Command);
                            Command = new Command();

                            break;
                        }
                    }
                    break;

                default:
                    break;
            }

            Refresh();
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:

                    SelectedShapes.Clear();

                    foreach (var shape in Shapes)
                    {
                        if (shape.Contains(e.Location))
                        {
                            SelectedShapes.Add(shape);
                        }
                    }

                    if (SelectedShapes.Count < 1)
                    {
                        return;
                    }

                    foreach (var shape in Shapes)
                    {
                        if (shape.ID == SelectedShapes[SelectedShapes.Count() - 1].ID)
                        {
                            Command.Name = "Fill";
                            Command.Color = shape.Color;
                            shape.Color = Color;

                            var type = shape.GetType().Name;

                            switch (type)
                            {
                                case "Circle":
                                    var cloneCircle = new Circle(float.Parse(textBox1.Text),
                                        shape.X, shape.Y, shape.ID, shape.Color);
                                    Command.Item = cloneCircle;
                                    break;
                                case "Rectangle":
                                    var cloneRectangle = new Rectangle(float.Parse(textBox2.Text),
                                        float.Parse(textBox3.Text), shape.X, shape.Y, shape.ID, shape.Color);
                                    Command.Item = cloneRectangle;
                                    break;
                                case "Triangle":
                                    var cloneTriangle = new Triangle(float.Parse(textBox4.Text),
                                        float.Parse(textBox6.Text), float.Parse(textBox5.Text),
                                        shape.X, shape.Y, shape.ID, shape.Color);
                                    Command.Item = cloneTriangle;
                                    break;
                                default:
                                    break;
                            }

                            RedoCommands.Clear();
                            UndoCommands.Add(Command);
                            Command = new Command();

                            break;
                        }
                    }

                    SelectedShapes.Clear();

                    break;

                default:
                    break;
            }

            Color = Color.Transparent;
        }
    }
}

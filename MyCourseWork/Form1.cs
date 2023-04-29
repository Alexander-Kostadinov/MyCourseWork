using Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
        public int CurrentID { get; set; }
        public string ShapeType { get; set; }

        Color Color { get; set; }
        public Pen Pen { get; set; }
        public List<IDrawable> Shapes { get; set; }

        public Undo Undo { get; set; }
        public Redo Redo { get; set; }
        public Clear Clear { get; set; }
        public Remove Remove { get; set; }

        public Command Command { get; set; }
        public List<Command> UndoCommands { get; set; }
        public List<Command> RedoCommands { get; set; }


        public Form1()
        {
            InitializeComponent();

            ID = 0;
            ShapeType = string.Empty;

            Color = new Color();
            Pen = new Pen(Color.Black, 2);
            Shapes = new List<IDrawable>();

            Command = new Command();
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
            ID++;

            try
            {
                switch (ShapeType)
                {
                    case "Circle":
                        var circle = new Circle(float.Parse(textBox1.Text), x, y, ID, flowLayoutPanel1.BackColor.ToString());
                        Command.Item = circle;
                        Shapes.Add(circle);
                        textBox7.Text = circle.Perimeter.ToString();
                        textBox8.Text = circle.Surface.ToString();
                        break;
                    case "Triangle":
                        var triangle = new Triangle(float.Parse(textBox4.Text),
                            float.Parse(textBox6.Text), float.Parse(textBox5.Text),
                            x, y, ID, flowLayoutPanel1.BackColor.ToString());
                        Command.Item = triangle;
                        Shapes.Add(triangle);
                        textBox7.Text = triangle.Perimeter.ToString();
                        textBox8.Text = triangle.Surface.ToString();
                        break;
                    case "Rectangle":
                        var rectangle = new Shapes.Rectangle(float.Parse(textBox2.Text),
                            float.Parse(textBox3.Text), x, y, ID, flowLayoutPanel1.BackColor.ToString());
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

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            var selected = Shapes.Where(x => x.ID < 0).FirstOrDefault();

            if (selected == null)
            {
                return;
            }

            Command.Name = "Fill";
            Command.Color = Color.FromName(selected.Color);
            selected.Color = flowLayoutPanel1.BackColor.Name;

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

                    RedoCommands.Clear();

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

                    switch (type)
                    {
                        case "Circle":
                            var cloneCircle = new Circle(shape.FirstSide, shape.X, shape.Y, shape.ID, shape.Color);
                            Command.Item = cloneCircle;
                            break;
                        case "Rectangle":
                            var cloneRectangle = new Shapes.Rectangle(shape.FirstSide,
                                shape.SecondSide, shape.X, shape.Y, shape.ID, shape.Color);
                            Command.Item = cloneRectangle;
                            break;
                        case "Triangle":
                            var cloneTriangle = new Triangle(shape.FirstSide,
                                shape.SecondSide, shape.ThirdSide, shape.X, shape.Y, shape.ID, shape.Color);
                            Command.Item = cloneTriangle;
                            break;
                        default:
                            break;
                    }

                    RedoCommands.Clear();

                    Command.Name = "Move";
                    UndoCommands.Add(Command);
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
                var type = shape.GetType().Name;

                Color = Color.FromName(shape.Color);

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
                        var points = shape.Draw(shape.X, shape.Y);

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

        private void button7_Click(object sender, EventArgs e)
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

                        Refresh();
                    }

                    sr.Close();
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

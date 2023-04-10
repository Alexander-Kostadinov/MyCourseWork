using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public abstract class Shape : IDrawable
    {
        private float x;
        private float y;
        private int id;
        protected double surface;
        protected double perimeter;

        protected Pen Pen { get; }
        protected SolidBrush Brush { get; }
        public float X
        {
            get => x;
            set => x = value;
        }
        public float Y
        {
            get => y;
            set => y = value;
        }
        public int ID 
        {
            get => id;
            set => id = value;
        }
        public Color Color
        {
            get => Brush.Color;
            set => Brush.Color = value;
        }

        public double Surface => Math.Round(CalculateSurface(), 2);
        public double Perimeter => Math.Round(CalculatePerimeter(), 2);


        protected Shape(float x, float y, int id, Color color)
        {
            X = x;
            Y = y;
            ID = id;
            Pen = new Pen(Color.Black,2);
            Brush = new SolidBrush(color);
        }

        public abstract double CalculateSurface();
        public abstract double CalculatePerimeter();
        public abstract void Draw(Graphics graphics, float x, float y, Color color);
        public abstract void Fill(Graphics graphics, float x, float y, Color color);
        public abstract bool Contains(Point point);
    }
}

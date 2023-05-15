using System;

namespace Shapes
{
    public abstract class Shape : IDrawable
    {
        private int id;
        private float x;
        private float y;
        private string color;
        protected double surface;
        protected double perimeter;

        public int ID
        {
            get => id;
            set => id = value;
        }
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

        public string Color
        {
            get => color;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("This color doesn't exist!");
                }

                color = value;
            }
        }

        public double Surface => Math.Round(CalculateSurface(), 2);
        public double Perimeter => Math.Round(CalculatePerimeter(), 2);

        protected Shape(float x, float y, int id, string color)
        {
            X = x;
            Y = y;
            ID = id;
            Color = color;
        }

        public abstract double CalculateSurface();
        public abstract double CalculatePerimeter();
        public abstract bool Contains(Point point);
        public abstract Point[] GetPoints(float x, float y);
    }
}

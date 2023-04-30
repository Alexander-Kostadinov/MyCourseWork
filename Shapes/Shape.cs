using System;

namespace Shapes
{
    public abstract class Shape : IDrawable
    {
        private int id;
        private float x;
        private float y;
        protected float firstSide;
        protected float secondSide;
        protected float thirdSide;
        protected double surface;
        protected double perimeter;

        public float FirstSide
        {
            get => firstSide;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("You should enter positive number!");
                }

                firstSide = value;
            }
        }

        public float SecondSide
        {
            get => secondSide;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("You should enter positive number!");
                }

                secondSide = value;
            }
        }
        public float ThirdSide
        {
            get => thirdSide;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("You should enter positive number!");
                }

                thirdSide = value;
            }
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
        public int ID 
        {
            get => id;
            set => id = value;
        }

        public string Color { get; set; }

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
        public abstract Point[] Draw(float x, float y);
        public abstract Point[] Fill(float x, float y);
    }
}

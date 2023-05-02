using System;

namespace Shapes
{
    public class Triangle : Shape
    {
        public Triangle(float a, float b, float c, float x, float y, int id, string color) 
            : base(x, y, id, color)
        {
            FirstSide = a;
            SecondSide = b;
            ThirdSide = c;
            IsPossibleTriangle(firstSide, secondSide, thirdSide);
        }

        private void IsPossibleTriangle(float a, float b, float c)
        {
            if (a >= b + c)
            {
                throw new Exception("Triangle with this sides cannot exists!");
            }
            else if (b >= a + c)
            {
                throw new Exception("Triangle with this sides cannot exists!");
            }
            else if (c >= a + b)
            {
                throw new Exception("Triangle with this sides cannot exists!");
            }
        }

        public override double CalculateSurface()
        {
            double p = (firstSide + secondSide + thirdSide) / 2;
            surface = Math.Sqrt(p * (p - firstSide) * (p - secondSide) * (p - thirdSide));
            return surface;
        }
        public override double CalculatePerimeter()
        {
            perimeter = firstSide + secondSide + thirdSide;
            return perimeter;
        }

        public override Point[] GetPoints(float x, float y)
        {
            var h = 2 * Surface / thirdSide;
            var distanceToA = Math.Sqrt((secondSide * secondSide) - (h * h));
            var distanceToB = Math.Sqrt((firstSide * firstSide) - (h * h));
            var pointC = new Point((int)x, (int)y);
            var pointA = new Point((int)(x - distanceToA), (int)(y + h));
            var pointB = new Point((int)(x + distanceToB), (int)(y + h));

            if (firstSide > thirdSide && firstSide > secondSide)
            {
                pointA.X = (int)(X + distanceToA);
                pointB.X = (int)(pointA.X + thirdSide);
            }
            else if (secondSide > thirdSide && secondSide > firstSide)
            {
                pointB.X = (int)(X - distanceToB);
                pointA.X = (int)(pointB.X - thirdSide);
            }

            var points = new Point[] { pointA, pointB, pointC };

            return points;
        }

        public override bool Contains(Point point)
        {
            var h = 2 * Surface / thirdSide;
            var distanceToA = Math.Sqrt((secondSide * secondSide) - (h * h));
            var distanceToB = Math.Sqrt((firstSide * firstSide) - (h * h));
            var pointA = new Point((int)(X - distanceToA), (int)(Y + h));
            var pointB = new Point((int)(X + distanceToB), (int)(Y + h));

            if (firstSide > thirdSide && firstSide > secondSide)
            {
                pointA.X = (int)(X + distanceToA);
                pointB.X = (int)(pointA.X + thirdSide);
            }
            else if (secondSide > thirdSide && secondSide > firstSide)
            {
                pointB.X = (int)(X - distanceToB);
                pointA.X = (int)(pointB.X - thirdSide);
            }

            bool result = false;

            if (point.Y + 88 < pointA.Y && point.Y + 88 > Y)
            {
                if (point.X <= X)
                {
                    var ratio = (X - pointA.X) / h;
                    var diff = pointA.Y - (point.Y + 88);
                    result = pointA.X + (diff * ratio) <= point.X;

                    if (result && pointB.X < X)
                    {
                        var diff2 = (point.Y + 88) - Y;
                        var ratio2 = (X - pointB.X) / h;
                        result = X - (diff2 * ratio2) >= point.X;
                        return result;
                    }

                    return result;
                }
                else if (point.X >= X)
                {
                    var ratio = (pointB.X - X) / h;
                    var diff = pointB.Y - (point.Y + 88);
                    result = pointB.X - (diff * ratio) >= point.X;

                    if (result && pointA.X > X)
                    {
                        var diff2 = (point.Y + 88) - Y;
                        var ratio2 = (pointA.X - X) / h;
                        result = X + (diff2 * ratio2) <= point.X;
                        return result;
                    }

                    return result;
                }
            }

            return result;
        }
    }
}

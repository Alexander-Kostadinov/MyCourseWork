﻿namespace Shapes
{
    public interface IDrawable
    {
        int ID { get; set; }
        float X { get; set; }
        float Y { get; set; }
        double Surface { get; }
        double Perimeter { get; }
        string Color { get; set; }

        double CalculateSurface();
        double CalculatePerimeter();
        bool Contains(Point point);
        Point[] GetPoints(float x, float y);
    }
}

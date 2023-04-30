namespace Shapes
{
    public interface IDrawable
    {
        int ID { get; set; }
        float X { get; set; }
        float Y { get; set; }
        double Surface { get; }
        double Perimeter { get; }
        string Color { get; set; }
        float FirstSide { get; set; }
        float SecondSide { get; set; }
        float ThirdSide { get; set; }

        double CalculateSurface();
        double CalculatePerimeter();

        bool Contains(Point point);
        Point[] Draw(float x, float y);
        Point[] Fill(float x, float y);
    }
}

using SplashKitSDK;
namespace ShapeDrawer;

public class MyRectangle : Shape
{
    private int _width;
    private int _height;

    public MyRectangle(Color color, float x, float y, int width, int height) : base(color)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public MyRectangle() : this(Color.Green, 0.0f, 0.0f, 100, 100)
    {

    }

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }

    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }


    public override void Draw()
    {
        if (Selected)
        {
            DrawOutline();
        }
        SplashKit.FillRectangle(Color, X, Y, Width, Height);
    }

    public override void DrawOutline()
    {
        SplashKit.DrawRectangle(Color.Black, X - 2, Y - 2, Width + 4, Height + 4);
    }

    public override bool IsAt(Point2D pt)
    {
        return (pt.X >= X) && (pt.X <= (X + Width)) &&
            (pt.Y >= Y) && (pt.Y <= (Y + Height));
    }

}

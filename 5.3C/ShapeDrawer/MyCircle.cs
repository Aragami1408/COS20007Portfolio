using SplashKitSDK;
namespace ShapeDrawer;

public class MyCircle : Shape
{
    private int _radius;

    public MyCircle(Color color, int radius) : base(color)
    {
        Radius = radius; 
    }

    public MyCircle() : this(Color.Blue, 50)
    {
    }

    public int Radius
    {
        get { return _radius; }
        set { _radius = value; }
    }

    public override void Draw()
    {
        if (Selected)
        {
            DrawOutline();
        }

        SplashKit.FillCircle(Color, X, Y, _radius);
    }

    public override void DrawOutline()
    {
        SplashKit.DrawCircle(Color.Black, X, Y, Radius + 2);
    }

    public override bool IsAt(Point2D pt)
    {
        double distanceX = Math.Abs(pt.X - X);
        double distanceY = Math.Abs(pt.Y - Y);

        return (distanceX <= Radius) && (distanceY <= Radius);
    }

         
    public override void SaveTo(StreamWriter writer)
    {
        writer.WriteLine("Circle");
        base.SaveTo(writer);
        writer.WriteLine(Radius);
    }
    
    public override void LoadFrom(StreamReader reader) {
        base.LoadFrom(reader);
        Radius = reader.ReadInteger();
    }
}

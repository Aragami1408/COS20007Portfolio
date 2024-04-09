using SplashKitSDK;
namespace ShapeDrawer;

public class MyLine : Shape
{
    private float _endX;
    private float _endY;

    public MyLine(Color color, float startX, float startY, float endX, float endY) : base(color)
    {
        X = startX;
        Y = startY;
        EndX = endX;
        EndY = endY;
    }

    public MyLine() : this(Color.Red, 0.0f, 0.0f, 50.0f, 20.0f)
    {
    }

    public float EndX
    {
        get {return _endX;}
        set {_endX = value;}
    }

    public float EndY
    {
        get {return _endY;}
        set {_endY = value;}
    }

    public override void Draw()
    {
        if (Selected)
            DrawOutline();

        SplashKit.DrawLine(Color, X, Y, X+EndX, Y+EndY);
    }

    public override void DrawOutline()
    {
        // SplashKit.DrawRectangle(Color.Black, X - 2, Y - 2, EndX + 4, EndY + 4);
        SplashKit.FillCircle(Color.Black, X, Y, 3);
        SplashKit.FillCircle(Color.Black, X+EndX, Y+EndY, 3);
    }

    public override bool IsAt(Point2D pt)
    {
        return (pt.X >= X) && (pt.X <= (X + EndX)) &&
            (pt.Y >= Y) && (pt.Y <= (Y + EndY));
    }

    public override void SaveTo(StreamWriter writer)
    {
        writer.WriteLine("Line");
        base.SaveTo(writer);
        writer.WriteLine(EndX);
        writer.WriteLine(EndY);
    }

    public override void LoadFrom(StreamReader reader) {
        base.LoadFrom(reader);
        EndX = reader.ReadInteger();
        EndY = reader.ReadInteger();
    }
}

using SplashKitSDK;
namespace ShapeDrawer;

public abstract class Shape
{
    private Color _color; 
    private float _x;
    private float _y;
    private bool _selected;

    public Shape(Color color)
    {
        this.Color = color;
        this.X = this.Y = 0.0f;
        
    }

    public Shape() : this(Color.Yellow)
    {

    }


    public Shape(Color color, int x, int y) {
        this.Color = color;
        this.X = x;
        this.Y = y;
    }

    public abstract void Draw();
    public abstract void DrawOutline();

    public abstract bool IsAt(Point2D pt);

    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public float X
    {
        get { return _x; }
        set { _x = value; }
    }

    public float Y
    {
        get { return _y; }
        set { _y = value; }
    }


    public bool Selected
    {
        get { return _selected; }
        set { _selected = value; }
    }

    public virtual void SaveTo(StreamWriter writer) 
    {
        writer.WriteColor(Color);
        writer.WriteLine(X);
        writer.WriteLine(Y);
    }

    public virtual void LoadFrom(StreamReader reader) {
        Color = reader.ReadColor();
        X = reader.ReadInteger();
        Y = reader.ReadInteger();
    }
}

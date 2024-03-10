using SplashKitSDK;
namespace ShapeDrawer;

public class Shape
{
    private Color _color; 
    private float _x;
    private float _y;
    private int _width;
    private int _height;

    public Shape()
    {
        this._color = Color.Green;
        this._x = this._y = 0.0f;
        this._width = this._height = 100;
    }

    public Shape(Color color, int x, int y, int width, int height) {
        this._color = color;
        this._x = x;
        this._y = y;
        this._width = width;
        this._height = height;
    }

    public void Draw()
    {
        SplashKit.FillRectangle(_color, _x, _y, _width, _height);
    }

    public bool IsAt(Point2D pt)
    {
        return (pt.X >= _x) && (pt.X <= (_x + _width)) &&
            (pt.Y >= _y) && (pt.Y <= (_y + _height));
    }

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


}

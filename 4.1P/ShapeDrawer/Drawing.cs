using  SplashKitSDK;
namespace ShapeDrawer;

public class Drawing
{
    private readonly List<Shape> _shapes;
    private Color _background;

    public Drawing(Color background) 
    {
        _shapes = new List<Shape>();
        _background = background;
    }

    public Drawing() : this(Color.White)
    {
    }

    public Color Background
    {
        get { return _background; }
        set { _background = value; }
    }

    public int ShapeCount
    {
        get { return _shapes.Count(); }
    }

    public void AddShape(Shape shape)
    {
        _shapes.Add(shape);
    }

    public bool RemoveShape(Shape shape)
    {
        return _shapes.Remove(shape);
    }

    public void SelectShapesAt(Point2D pt)
    {
        foreach (var s in _shapes)
        {
            if (s.IsAt(pt))
                s.Selected = !s.Selected;
        }
    }

    public List<Shape> SelectedShapes
    {

        get { 

            var result = new List<Shape>();

            foreach (var s in _shapes)
            {
                if (s.Selected)
                {
                    result.Add(s);
                }
            }
            return result; 
        }

    }

    public void Draw()
    {
        SplashKit.ClearScreen(_background); 

        foreach (var shape in _shapes)
        {
            shape.Draw();
        }
    }


}

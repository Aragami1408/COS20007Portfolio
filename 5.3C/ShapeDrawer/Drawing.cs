using System.IO;
using SplashKitSDK;
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

    public void Save(string filename)
    {
        StreamWriter writer = new StreamWriter(filename);
        try
        {
            writer.WriteColor(Background);
            writer.WriteLine(_shapes.Count);

            foreach (var s in _shapes) 
            {
                s.SaveTo(writer);
            }
        }
        finally
        {
            writer.Close();
        }

    }

    public void Load(string filename)
    {

        StreamReader reader = new StreamReader(filename);
        try 
        {
            Background = reader.ReadColor();

            int count = reader.ReadInteger();

            _shapes.Clear();

            Shape s;
            string? kind;
            for (int i = 0; i < count; i++) 
            {
                kind = reader.ReadLine();

                switch (kind) 
                {
                    case "Rectangle":
                        s = new MyRectangle();
                        break;
                    case "Circle":
                        s = new MyCircle();
                        break;
                    case "Line":
                        s = new MyLine();
                        break;
                    default:
                        throw new InvalidDataException("Unknown shape kind: " + kind);
                }

                s.LoadFrom(reader);
                AddShape(s);
            }
        }
        finally 
        {
            reader.Close();
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

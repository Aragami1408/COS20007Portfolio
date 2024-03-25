using System;
using SplashKitSDK;

namespace ShapeDrawer
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Shape Drawer", 800, 600);
            Drawing myDrawing = new Drawing();

            do
            {
                SplashKit.ProcessEvents();

                if (SplashKit.KeyTyped(KeyCode.SpaceKey)) {
                    myDrawing.Background = SplashKit.RandomColor();
                }

                if (SplashKit.MouseClicked(MouseButton.LeftButton)) 
                {
                    Shape shape = new Shape();
                    shape.X = SplashKit.MouseX();
                    shape.Y = SplashKit.MouseY();
                    myDrawing.AddShape(shape);
                }

                if (SplashKit.MouseClicked(MouseButton.RightButton))
                {
                    Point2D mousePos;
                    mousePos.X = SplashKit.MouseX();
                    mousePos.Y = SplashKit.MouseY();

                    myDrawing.SelectShapesAt(mousePos);
                }

                if (SplashKit.KeyTyped(KeyCode.BackspaceKey)) 
                {
                    var selectedShapes = myDrawing.SelectedShapes;

                    foreach(var shape in selectedShapes)
                    {
                        myDrawing.RemoveShape(shape);
                    }
                }


                myDrawing.Draw();


                SplashKit.RefreshScreen();
            } while (!window.CloseRequested);
        }
    }
}

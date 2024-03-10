using System;
using SplashKitSDK;

namespace ShapeDrawer
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Shape Drawer", 800, 600);

            Shape shape = new Shape(Color.Bisque, 50, 50, 100, 100);

            do
            {
                SplashKit.ProcessEvents();

                if (SplashKit.MouseClicked(MouseButton.LeftButton)) 
                {
                    shape.X = SplashKit.MouseX();
                    shape.Y = SplashKit.MouseY();
                }

                if (SplashKit.KeyTyped(KeyCode.SpaceKey)) {
                    Point2D pt;
                    pt.X = SplashKit.MouseX();
                    pt.Y = SplashKit.MouseY();
                    if (shape.IsAt(pt)) {
                        shape.Color = SplashKit.RandomColor();
                    }
                }

                SplashKit.ClearScreen();

                shape.Draw();

                SplashKit.RefreshScreen();
            } while (!window.CloseRequested);
        }
    }
}

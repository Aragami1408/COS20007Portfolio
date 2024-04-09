using System;
using SplashKitSDK;

namespace ShapeDrawer
{
    public class Program
    {
        private enum ShapeKind
        {
            Rectangle,
            Circle,
            Line
        }

        public static void Main()
        {
            Window window = new Window("Shape Drawer", 800, 600);
            Drawing myDrawing = new Drawing();

            ShapeKind kindToAdd = ShapeKind.Rectangle;

            do
            {
                SplashKit.ProcessEvents();

                if (SplashKit.KeyTyped(KeyCode.SpaceKey)) 
                {
                    myDrawing.Background = SplashKit.RandomColor();
                }

                if (SplashKit.KeyTyped(KeyCode.RKey))
                    kindToAdd = ShapeKind.Rectangle;
                if (SplashKit.KeyTyped(KeyCode.CKey))
                    kindToAdd = ShapeKind.Circle;
                if (SplashKit.KeyTyped(KeyCode.LKey))
                    kindToAdd = ShapeKind.Line;
                if (SplashKit.KeyTyped(KeyCode.SKey)) {
                    try {
                        myDrawing.Save("TestDrawing.txt");
                    }
                    catch (Exception e) {
                        Console.Error.WriteLine("Error Saving File: {0}", e.Message);
                    }
                }
                if (SplashKit.KeyTyped(KeyCode.OKey)) {
                    try {
                        myDrawing.Load("TestDrawing.txt");
                    }
                    catch (Exception e) {
                        Console.Error.WriteLine("Error Loading File: {0}", e.Message);
                    }
                }


                if (SplashKit.MouseClicked(MouseButton.LeftButton)) 
                {
                    Shape newShape = new MyRectangle();

                    switch (kindToAdd)
                    {
                        case ShapeKind.Rectangle:
                            newShape = new MyRectangle();
                            break;

                        case ShapeKind.Circle:
                            newShape = new MyCircle();
                            break;


                        case ShapeKind.Line:
                            newShape = new MyLine();
                            break;
                    }

                    newShape.X = SplashKit.MouseX();
                    newShape.Y = SplashKit.MouseY();


                    myDrawing.AddShape(newShape);
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

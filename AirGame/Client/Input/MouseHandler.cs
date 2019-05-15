using System.Collections;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class MouseHandler
    {
        public static Hashtable pressed = new Hashtable();
        public static Hashtable clicked = new Hashtable();
        public static int lastMouseX;
        public static int lastMouseY;
        public static int MouseX => Mouse.GetState().X;
        public static int MouseY => Mouse.GetState().Y;
        public static int Dx => MouseX - lastMouseX;
        public static int Dy => lastMouseY - MouseY;


        public static void Setup()
        {
            pressed.Add(MouseButton.Left, Mouse.GetState().IsButtonDown(MouseButton.Left));
            pressed.Add(MouseButton.Right, Mouse.GetState().IsButtonDown(MouseButton.Right));
            pressed.Add(MouseButton.Middle, Mouse.GetState().IsButtonDown(MouseButton.Middle));
            clicked.Add(MouseButton.Left, Mouse.GetState().IsButtonDown(MouseButton.Left));
            clicked.Add(MouseButton.Right, Mouse.GetState().IsButtonDown(MouseButton.Right));
            clicked.Add(MouseButton.Middle, Mouse.GetState().IsButtonDown(MouseButton.Middle));
        }

        public static void Update()
        {
            var left = Mouse.GetState().IsButtonDown(MouseButton.Left);
            var right = Mouse.GetState().IsButtonDown(MouseButton.Right);
            var middle = Mouse.GetState().IsButtonDown(MouseButton.Middle);
            clicked[MouseButton.Left] = !(bool) pressed[MouseButton.Left] && left;
            clicked[MouseButton.Right] = !(bool) pressed[MouseButton.Right] && right;
            clicked[MouseButton.Middle] = !(bool) pressed[MouseButton.Middle] && middle;
            pressed[MouseButton.Left] = left;
            pressed[MouseButton.Right] = right;
            pressed[MouseButton.Middle] = middle;
            lastMouseX = MouseX;
            lastMouseY = MouseY;
        }
    }
}
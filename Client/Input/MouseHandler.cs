using System.Collections;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class MouseHandler
    {
        public static int MouseX => Mouse.GetState().X;
        public static int MouseY => Mouse.GetState().Y;

        public static Hashtable Pressed = new Hashtable();
        public static Hashtable Clicked = new Hashtable();

        public static void Setup()
        {
            Pressed.Add(MouseButton.Left, Mouse.GetState().IsButtonDown(MouseButton.Left));
            Pressed.Add(MouseButton.Right, Mouse.GetState().IsButtonDown(MouseButton.Right));
            Pressed.Add(MouseButton.Middle, Mouse.GetState().IsButtonDown(MouseButton.Middle));
            Clicked.Add(MouseButton.Left, Mouse.GetState().IsButtonDown(MouseButton.Left));
            Clicked.Add(MouseButton.Right, Mouse.GetState().IsButtonDown(MouseButton.Right));
            Clicked.Add(MouseButton.Middle, Mouse.GetState().IsButtonDown(MouseButton.Middle));
        }

        public static void Update()
        {
            bool left = Mouse.GetState().IsButtonDown(MouseButton.Left);
            bool right = Mouse.GetState().IsButtonDown(MouseButton.Right);
            bool middle = Mouse.GetState().IsButtonDown(MouseButton.Middle);
            Clicked[MouseButton.Left] = !(bool) Pressed[MouseButton.Left] && left;
            Clicked[MouseButton.Right] = !(bool) Pressed[MouseButton.Right] && right;
            Clicked[MouseButton.Middle] = !(bool) Pressed[MouseButton.Middle] && middle;
            Pressed[MouseButton.Left] = left;
            Pressed[MouseButton.Right] = right;
            Pressed[MouseButton.Middle] = middle;
        }
    }
}
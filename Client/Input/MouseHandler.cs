using System.Collections;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class MouseHandler
    {
        public static int MouseX => Mouse.GetState().X;
        public static int MouseY => Mouse.GetState().Y;

        public static Hashtable _pressed = new Hashtable();
        public static Hashtable _clicked = new Hashtable();

        public static void Setup()
        {
            _pressed.Add(MouseButton.Left, Mouse.GetState().IsButtonDown(MouseButton.Left));
            _pressed.Add(MouseButton.Right, Mouse.GetState().IsButtonDown(MouseButton.Right));
            _pressed.Add(MouseButton.Middle, Mouse.GetState().IsButtonDown(MouseButton.Middle));
            _clicked.Add(MouseButton.Left, Mouse.GetState().IsButtonDown(MouseButton.Left));
            _clicked.Add(MouseButton.Right, Mouse.GetState().IsButtonDown(MouseButton.Right));
            _clicked.Add(MouseButton.Middle, Mouse.GetState().IsButtonDown(MouseButton.Middle));
        }

        public static void Update()
        {
            bool left = Mouse.GetState().IsButtonDown(MouseButton.Left);
            bool right = Mouse.GetState().IsButtonDown(MouseButton.Right);
            bool middle = Mouse.GetState().IsButtonDown(MouseButton.Middle);
            _clicked[MouseButton.Left] = !(bool) _pressed[MouseButton.Left] && left;
            _clicked[MouseButton.Right] = !(bool) _pressed[MouseButton.Right] && right;
            _clicked[MouseButton.Middle] = !(bool) _pressed[MouseButton.Middle] && middle;
            _pressed[MouseButton.Left] = left;
            _pressed[MouseButton.Right] = right;
            _pressed[MouseButton.Middle] = middle;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyboardHandler
    {
        public static Hashtable Pressed = new Hashtable();
        public static Hashtable Clicked = new Hashtable();
        public static List<Key> Keys = new List<Key>();

        public static void RegisterKey(Key key)
        {
            KeyboardState state = Keyboard.GetState();
            if (!Keys.Contains(key))
            {
                Keys.Add(key);
                Pressed.Add(key, state.IsKeyDown(key));
                Clicked.Add(key, state.IsKeyDown(key));
            }
        }

        public static void Update()
        {
            KeyboardState state = Keyboard.GetState();
            foreach (var key in Keys)
            {
                bool old = state.IsKeyDown(key);
                Clicked[key] = !(bool) Pressed[key] && old;
                Pressed[key] = old;
            }
        }
    }
}
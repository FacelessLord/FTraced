using System.Collections;
using System.Collections.Generic;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyboardHandler
    {
        public static Hashtable pressed = new Hashtable();
        public static Hashtable clicked = new Hashtable();
        public static List<Key> keys = new List<Key>();

        public static void RegisterKey(Key key)
        {
            var state = Keyboard.GetState();
            if (!keys.Contains(key))
            {
                keys.Add(key);
                pressed.Add(key, state.IsKeyDown(key));
                clicked.Add(key, state.IsKeyDown(key));
            }
        }

        public static void Update()
        {
            var state = Keyboard.GetState();
            foreach (var key in keys)
            {
                var old = state.IsKeyDown(key);
                clicked[key] = !(bool) pressed[key] && old;
                pressed[key] = old;
            }
        }
    }
}
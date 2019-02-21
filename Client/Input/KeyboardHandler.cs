using System.Collections;
using System.Collections.Generic;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyboardHandler
    {
        public static Hashtable _pressed = new Hashtable();
        public static Hashtable _clicked = new Hashtable();
        public static List<Key> _keys = new List<Key>();

        public static void RegisterKey(Key key)
        {
            KeyboardState state = Keyboard.GetState();
            if (!_keys.Contains(key))
            {
                _keys.Add(key);
                _pressed.Add(key, state.IsKeyDown(key));
                _clicked.Add(key, state.IsKeyDown(key));
            }
        }

        public static void Update()
        {
            KeyboardState state = Keyboard.GetState();
            foreach (var key in _keys)
            {
                bool old = state.IsKeyDown(key);
                _clicked[key] = !(bool) _pressed[key] && old;
                _pressed[key] = old;
            }
        }
    }
}
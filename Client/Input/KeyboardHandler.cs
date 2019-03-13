using System.Collections;
using System.Collections.Generic;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyboardHandler
    {
        public static readonly Hashtable PressedKeys = new Hashtable();
        public static readonly Hashtable ClickedKeys = new Hashtable();
        public static List<Key> keys = new List<Key>();

        public static void RegisterKey(Key key)
        {
            var state = Keyboard.GetState();
            if (!keys.Contains(key))
            {
                keys.Add(key);
                PressedKeys.Add(key, state.IsKeyDown(key));
                ClickedKeys.Add(key, state.IsKeyDown(key));
            }
        }

        public static bool SetPressed(Key key, bool pressed)
        {
            if (keys.Contains(key))
                if (!PressedKeys.ContainsKey(key))
                {
                    PressedKeys.Add(key, pressed);
                }
                else
                {
                    var oldValue = (bool) PressedKeys[key];
                    PressedKeys[key] = pressed;
                    return oldValue;
                }

            return false;
        }

        public static void SetClicked(Key key, bool clicked)
        {
            if (keys.Contains(key))
                if (!ClickedKeys.ContainsKey(key))
                    ClickedKeys.Add(key, clicked);
                else
                    ClickedKeys[key] = clicked;
        }

        public static void Update()
        {
            var state = Keyboard.GetState();
            foreach (var key in keys) ClickedKeys[key] = false;
        }
    }
}
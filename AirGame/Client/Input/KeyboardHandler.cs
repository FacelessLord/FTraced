using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GlLib.Common.Entities;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyboardHandler
    {
        public static readonly Hashtable PressedKeys = new Hashtable();
        public static readonly Hashtable ClickedKeys = new Hashtable();
        public static List<Key> keys = new List<Key>();

        internal static ConcurrentBag<Action<EntityPlayer>> implementedActions = new ConcurrentBag<Action<EntityPlayer>>();
        internal static List<BindPattern> patterns = new List<BindPattern>();


        public static void RegisterKey(Key _key)
        {
            if (!keys.Contains(_key))
            {
                keys.Add(_key);
                PressedKeys.Add(_key, false);
                ClickedKeys.Add(_key, false);
            }
        }

        public static bool SetPressed(Key _key, bool _pressed)
        {
            if (keys.Contains(_key))
                if (!PressedKeys.ContainsKey(_key))
                {
                    PressedKeys.Add(_key, _pressed);
                }
                else
                {
                    var oldValue = (bool) PressedKeys[_key];
                    PressedKeys[_key] = _pressed;
                    return oldValue;
                }

            return false;
        }

        public static void SetClicked(Key _key, bool _clicked)
        {
            if (keys.Contains(_key))
                if (!ClickedKeys.ContainsKey(_key))
                    ClickedKeys.Add(_key, _clicked);
                else
                    ClickedKeys[_key] = _clicked;
        }


        public static void Update()
        {
            foreach (var key in keys) ClickedKeys[key] = false;
        }
    }
}
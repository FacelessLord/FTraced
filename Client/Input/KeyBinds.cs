using System;
using System.Collections.Generic;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyBinds
    {
        public static Dictionary<Key, Delegate> _binds = new Dictionary<Key, Delegate>();
        public static Dictionary<Delegate, Key> _delegates = new Dictionary<Delegate, Key>();

        public static void Register()
        {
            Bind(Key.W, _moveUp);
            Bind(Key.A, _moveLeft);
            Bind(Key.S, _moveDown);
            Bind(Key.D, _moveRight);
        }

        public static void Update(Player p)
        {
            p._acceleration = new PlanarVector();
            foreach (var key in KeyboardHandler._keys)
            {
                if (_binds.ContainsKey(key) && (bool) KeyboardHandler._pressed[key])
                {
                    _binds[key].DynamicInvoke(p);
                }
            }
        }

        public static void Bind(Key key, Delegate @delegate)
        {
            _binds.Add(key, @delegate);
            _delegates.Add(@delegate, key);
            KeyboardHandler.RegisterKey(key);
        }

        public static void Rebind(Key key, Delegate @delegate)
        {
            List<Key> toRemove = new List<Key>();
            foreach (var keyh in KeyboardHandler._keys)
            {
                if (_binds[keyh] == @delegate)
                {
                    toRemove.Add(keyh);
                }
            }

            foreach (var keyh in toRemove)
            {
                _binds.Remove(keyh);
            }

            toRemove.Clear();

            KeyboardHandler.RegisterKey(key);
            _binds.Add(key, @delegate);
            if (_delegates.ContainsKey(@delegate))
            {
                _delegates[@delegate] = key;
            }
            else
            {
                _delegates.Add(@delegate, key);
            }
        }

        public static MoveLeftDelegate _moveLeft = (p) => p._acceleration += new PlanarVector(-p._accelValue, -p._accelValue);
        public static MoveUpDelegate _moveUp = (p) => p._acceleration += new PlanarVector(-p._accelValue, p._accelValue);
        public static MoveRightDelegate _moveRight = (p) => p._acceleration += new PlanarVector(p._accelValue, p._accelValue);
        public static MoveDownDelegate _moveDown = (p) => p._acceleration += new PlanarVector(p._accelValue, -p._accelValue);
    }

    public delegate void MoveLeftDelegate(Player p);

    public delegate void MoveUpDelegate(Player p);

    public delegate void MoveRightDelegate(Player p);

    public delegate void MoveDownDelegate(Player p);
}
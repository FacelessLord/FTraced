using System;
using System.Collections.Generic;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyBinds
    {
        public static Dictionary<Key, Delegate> binds = new Dictionary<Key, Delegate>();
        public static Dictionary<Delegate, Key> delegates = new Dictionary<Delegate, Key>();

        public static void Register()
        {
            Bind(Key.W, moveUp);
            Bind(Key.A, moveLeft);
            Bind(Key.S, moveDown);
            Bind(Key.D, moveRight);
        }

        public static void Update(Player p)
        {
            foreach (var key in KeyboardHandler.Keys)
            {
                if (binds.ContainsKey(key) && (bool) KeyboardHandler.Pressed[key])
                {
                    binds[key].DynamicInvoke(p);
                }
            }
        }

        public static void Bind(Key key, Delegate @delegate)
        {
            binds.Add(key, @delegate);
            delegates.Add(@delegate, key);
            KeyboardHandler.RegisterKey(key);
        }

        public static void Rebind(Key key, Delegate @delegate)
        {
            List<Key> toRemove = new List<Key>();
            foreach (var keyh in KeyboardHandler.Keys)
            {
                if (binds[keyh] == @delegate)
                {
                    toRemove.Add(keyh);
                }
            }

            foreach (var keyh in toRemove)
            {
                binds.Remove(keyh);
            }

            toRemove.Clear();

            KeyboardHandler.RegisterKey(key);
            binds.Add(key, @delegate);
            if (delegates.ContainsKey(@delegate))
            {
                delegates[@delegate] = key;
            }
            else
            {
                delegates.Add(@delegate, key);
            }
        }

        public static MoveLeftDelegate moveLeft = (p) =>
        {
            Console.WriteLine("Left");
            p._acceleration += new PlanarVector(-p._accelValue, -p._accelValue);
        };
        public static MoveUpDelegate moveUp = (p) => p._acceleration += new PlanarVector(-p._accelValue, p._accelValue);
        public static MoveRightDelegate moveRight = (p) => p._acceleration += new PlanarVector(p._accelValue, p._accelValue);
        public static MoveDownDelegate moveDown = (p) => p._acceleration += new PlanarVector(p._accelValue, -p._accelValue);
    }

    public delegate void MoveLeftDelegate(Player p);

    public delegate void MoveUpDelegate(Player p);

    public delegate void MoveRightDelegate(Player p);

    public delegate void MoveDownDelegate(Player p);
}
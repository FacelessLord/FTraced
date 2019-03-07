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

        public static MoveLeftDelegate moveLeft = p =>
            p.acceleration += new PlanarVector(-p.accelValue, -p.accelValue);

        public static MoveUpDelegate moveUp = p => p.acceleration += new PlanarVector(-p.accelValue, p.accelValue);

        public static MoveRightDelegate moveRight = p =>
            p.acceleration += new PlanarVector(p.accelValue, p.accelValue);

        public static MoveDownDelegate moveDown = p =>
            p.acceleration += new PlanarVector(p.accelValue, -p.accelValue);

        public static void Register()
        {
            Bind(Key.W, moveUp);
            Bind(Key.A, moveLeft);
            Bind(Key.S, moveDown);
            Bind(Key.D, moveRight);
        }

        public static void Update(Player p)
        {
            p.acceleration = new PlanarVector();
            foreach (var key in KeyboardHandler.keys)
                if (binds.ContainsKey(key) && (bool) KeyboardHandler.pressed[key])
                    binds[key].DynamicInvoke(p);
        }

        public static void Bind(Key key, Delegate @delegate)
        {
            binds.Add(key, @delegate);
            delegates.Add(@delegate, key);
            KeyboardHandler.RegisterKey(key);
        }

        public static void Rebind(Key key, Delegate @delegate)
        {
            var toRemove = new List<Key>();
            foreach (var keyh in KeyboardHandler.keys)
                if (binds[keyh] == @delegate)
                    toRemove.Add(keyh);

            foreach (var keyh in toRemove) binds.Remove(keyh);

            toRemove.Clear();

            KeyboardHandler.RegisterKey(key);
            binds.Add(key, @delegate);
            if (delegates.ContainsKey(@delegate))
                delegates[@delegate] = key;
            else
                delegates.Add(@delegate, key);
        }
    }

    public delegate void MoveLeftDelegate(Player p);

    public delegate void MoveUpDelegate(Player p);

    public delegate void MoveRightDelegate(Player p);

    public delegate void MoveDownDelegate(Player p);
}
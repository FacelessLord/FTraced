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

        public static MoveLeftDelegate moveLeft = _p =>
        {
            _p.velocity += new PlanarVector(-_p.accelerationValue, -_p.accelerationValue);
            _p.CheckVelocity();
        };

        public static MoveUpDelegate moveUp = _p =>
        {
            _p.velocity += new PlanarVector(-_p.accelerationValue, _p.accelerationValue);
            _p.CheckVelocity();
        };

        public static MoveRightDelegate moveRight = _p =>
        {
            _p.velocity += new PlanarVector(_p.accelerationValue, _p.accelerationValue);
            _p.CheckVelocity();
        };

        public static MoveDownDelegate moveDown = _p =>
        {
            _p.velocity += new PlanarVector(_p.accelerationValue, -_p.accelerationValue);
            _p.CheckVelocity();
        };

        public static void Register()
        {
            Bind(Key.Up, moveUp);
            Bind(Key.Left, moveLeft);
            Bind(Key.Down, moveDown);
            Bind(Key.Right, moveRight);
        }

        public static void Bind(Key _key, Delegate _delegate)
        {
            binds.Add(_key, _delegate);
            delegates.Add(_delegate, _key);
            KeyboardHandler.RegisterKey(_key);
        }
    }

    public delegate void MoveLeftDelegate(Player _p);

    public delegate void MoveUpDelegate(Player _p);

    public delegate void MoveRightDelegate(Player _p);

    public delegate void MoveDownDelegate(Player _p);
}
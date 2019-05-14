using System;
using System.Collections.Generic;
using GlLib.Client.Graphic.Gui;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyBinds
    {
        public static Dictionary<Key, Action<Player>> binds = new Dictionary<Key, Action<Player>>();
        public static Dictionary<Key, Action<Player>> clickBinds = new Dictionary<Key, Action<Player>>();

        public static Action<Player> moveLeft = _p =>
        {
            _p.velocity += new PlanarVector(-_p.accelerationValue, 0);
            _p.CheckVelocity();
        };

        public static Action<Player> moveUp = _p =>
        {
            _p.velocity += new PlanarVector(0, -_p.accelerationValue);
            _p.CheckVelocity();
        };

        public static Action<Player> moveRight = _p =>
        {
            _p.velocity += new PlanarVector(_p.accelerationValue, 0);
            _p.CheckVelocity();
        };

        public static Action<Player> moveDown = _p =>
        {
            _p.velocity += new PlanarVector(0, _p.accelerationValue);
            _p.CheckVelocity();
        };

        public static Action<Player> openInventory =
            _p => Proxy.GetWindow().TryOpenGui(new PlayerFrameInventoryGuiFrame(_p));

        public static Action<Player> openMainMenu = _p =>
            Proxy.GetWindow().TryOpenGui(new GuiMainMenu());

        public static Action<Player> exit = _p => Proxy.Exit = true;

        public static void Register()
        {
            Bind(Key.Up, moveUp);
            Bind(Key.Left, moveLeft);
            Bind(Key.Down, moveDown);
            Bind(Key.Right, moveRight);
            BindClick(Key.I, openInventory);
            BindClick(Key.M, openMainMenu);
            BindClick(Key.Escape, exit);
        }

        public static void Bind(Key _key, Action<Player> _action)
        {
            binds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
        }

        public static void BindClick(Key _key, Action<Player> _action)
        {
            clickBinds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
        }
    }
}
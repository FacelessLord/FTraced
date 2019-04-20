using System;
using System.Collections.Generic;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyBinds
    {
        public static Dictionary<Key, Action<Player>> binds = new Dictionary<Key, Action<Player>>();
        public static Dictionary<Key, Action<Player>> clickBinds = new Dictionary<Key, Action<Player>>();

        public static Action<Player> moveLeft = _p =>
        {
            _p.velocity += new PlanarVector(-_p.accelerationValue, -_p.accelerationValue);
            _p.CheckVelocity();
        };

        public static Action<Player> moveUp = _p =>
        {
            _p.velocity += new PlanarVector(-_p.accelerationValue, _p.accelerationValue);
            _p.CheckVelocity();
        };

        public static Action<Player> moveRight = _p =>
        {
            _p.velocity += new PlanarVector(_p.accelerationValue, _p.accelerationValue);
            _p.CheckVelocity();
        };

        public static Action<Player> moveDown = _p =>
        {
            _p.velocity += new PlanarVector(_p.accelerationValue, -_p.accelerationValue);
            _p.CheckVelocity();
        };

        public static Action<Player> openInventory = _p =>
        {
            if (GraphicWindow.instance.gui == null)
            {
                GraphicWindow.instance.gui = new Gui();
                GraphicWindow.instance.gui
                    .AddRectangle(GraphicWindow.instance.Width / 38, GraphicWindow.instance.Height / 9,
                        4 * GraphicWindow.instance.Width / 9, 2 * GraphicWindow.instance.Height / 5,
                        Color.FromArgb(127, 200, 200, 200));
                GraphicWindow.instance.gui.AddText("abcdefghijklmopqrstuvwxyz".ToUpper(), 100, 100, 200, 20);

            }
            else
            {
                GraphicWindow.instance.gui = null;
            }
        };

        public static void Register()
        {
            Bind(Key.Up, moveUp);
            Bind(Key.Left, moveLeft);
            Bind(Key.Down, moveDown);
            Bind(Key.Right, moveRight);
            BindClick(Key.I, openInventory);
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
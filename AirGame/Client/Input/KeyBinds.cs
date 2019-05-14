using System;
using System.Collections.Generic;
using System.Linq;
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
        public static Dictionary<Action<Player>, String> delegateNames = new Dictionary<Action<Player>, String>();

        public static Action<Player> moveLeft = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.velocity += new PlanarVector(-_p.accelerationValue, 0);
                _p.CheckVelocity();
            }
        };

        public static Action<Player> moveUp = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.velocity += new PlanarVector(0, -_p.accelerationValue);
                _p.CheckVelocity();
            }
        };

        public static Action<Player> moveRight = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.velocity += new PlanarVector(_p.accelerationValue, 0);
                _p.CheckVelocity();
            }
        };

        public static Action<Player> moveDown = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.velocity += new PlanarVector(0, _p.accelerationValue);
                _p.CheckVelocity();
            }
        };

        public static Action<Player> openInventory = _p =>
        {
            if (Proxy.GetWindow().serverStarted)
                Proxy.GetWindow().TryOpenGui(new PlayerFrameInventoryGuiFrame(_p));
        };

        public static Action<Player> openIngameMenu = _p =>
        {
            if (Proxy.GetWindow().serverStarted)
                Proxy.GetWindow().TryOpenGui(new GuiIngameMenu());
        };

        public static Action<Player> spawnSlime = _p =>
        {
            if (Proxy.GetWindow().serverStarted)
                _p.worldObj.SpawnEntity(new EntitySlime(_p.worldObj, _p.Position));
        };

        public static Action<Player> exit = _p => Proxy.Exit = true;

        public static Action<Player> attack = _p =>
        {
            var entities = _p.worldObj.GetEntitiesWithinAaBb(_p.GetAaBb());
            entities.Where(_e => _e is EntityLiving && _e != _p).Cast<EntityLiving>().ToList()
                .ForEach(_e => _e.DealDamage(30));
        };

        public static void Register()
        {
            Bind(Key.W, moveUp, "move.up");
            Bind(Key.A, moveLeft, "move.left");
            Bind(Key.S, moveDown, "move.down");
            Bind(Key.D, moveRight, "move.right");
            BindClick(Key.I, openInventory, "gui.inv");
            BindClick(Key.Escape, openIngameMenu, "gui.menu");
            BindClick(Key.Grave, exit, "exit");
            BindClick(Key.Space, attack, "world.attack");
            BindClick(Key.G, spawnSlime, "world.spawn.slime");
        }

        public static void Bind(Key _key, Action<Player> _action, string _name)
        {
            binds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
            delegateNames.Add(_action, _name);
        }

        public static void BindClick(Key _key, Action<Player> _action, string _name)
        {
            clickBinds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
            delegateNames.Add(_action, _name);
        }

        public static void RebindClick(Key _key, Action<Player> _action)
        {
            clickBinds.Remove(clickBinds.Keys.Single(_k => clickBinds[_k] == _action));
            clickBinds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
        }

        public static void Rebind(Key _key, Action<Player> _action)
        {
            binds.Remove(binds.Keys.Single(_k => binds[_k] == _action));
            binds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Graphic.Gui;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Entities.Items;
using GlLib.Common.Map;
using GlLib.Common.SpellCastSystem;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyBinds
    {
        public static Dictionary<Key, Action<Player>> binds = new Dictionary<Key, Action<Player>>();
        public static Dictionary<Key, Action<Player>> clickBinds = new Dictionary<Key, Action<Player>>();
        public static Dictionary<Action<Player>, string> delegateNames = new Dictionary<Action<Player>, string>();

        public static Action<Player> moveLeft = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.direction = Direction.Left;
                _p.SetState(EntityState.Walk, 3);
                _p.velocity += new PlanarVector(-_p.accelerationValue, 0);
            }
        };

        public static Action<Player> moveUp = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.SetState(EntityState.Walk, 3);
                _p.velocity += new PlanarVector(0, -_p.accelerationValue);
            }
        };

        public static Action<Player> setBlock = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                var chunkX = _p.Position.Ix / 16;
                var chunkY = _p.Position.Iy / 16;

                var blockX = _p.Position.Ix % 16;
                var blockY = _p.Position.Iy % 16;

                _p.worldObj[chunkX, chunkY][blockX, blockY] =
                    (TerrainBlock) Proxy.GetRegistry().blocksById[0];
            }
        };

        public static Action<Player> saveWorld = _p =>
        {
            WorldManager.SaveWorld(_p.worldObj);
        };


        public static Action<Player> moveRight = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.SetState(EntityState.Walk, 3);
                _p.direction = Direction.Right;
                _p.velocity += new PlanarVector(_p.accelerationValue, 0);
            }
        };

        public static Action<Player> moveDown = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                _p.SetState(EntityState.Walk, 3);
                _p.velocity += new PlanarVector(0, _p.accelerationValue);
            }
        };

        public static Action<Player> openInventory = _p =>
        {
            if (Proxy.GetWindow().serverStarted)
                Proxy.GetWindow().TryOpenGui(new PlayerInventoryGui(_p));
        };

        public static Action<Player> openIngameMenu = _p =>
        {
            if (Proxy.GetWindow().serverStarted)
                Proxy.GetWindow().TryOpenGui(new GuiIngameMenu());
        };

        public static Action<Player> openChat = _p =>
        {
            if (Proxy.GetWindow().serverStarted)
                Proxy.GetWindow().TryOpenGui(new GuiChat());
        };

        public static Action<Player> spawnSlime = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
                _p.worldObj.SpawnEntity(new EntitySlime(_p.worldObj, _p.Position));
        };

        public static Action<Player> spawnBat = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
                _p.worldObj.SpawnEntity(new Bat(_p.worldObj, _p.Position));
        };

        public static Action<Player> exit = _p => Proxy.Exit = true;

        public static Action<Player> spellFire = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled()) _p.spells.OnUpdate(ElementType.Fire);
        };

        public static Action<Player> spellAir = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled()) _p.spells.OnUpdate(ElementType.Air);
        };

        public static Action<Player> spellEarth = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled()) _p.spells.OnUpdate(ElementType.Earth);
        };

        public static Action<Player> spellWater = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled()) _p.spells.OnUpdate(ElementType.Water);
        };


        public static Action<Player> attack = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                if (!(_p.state is EntityState.AttackInterrupted))
                {
                    if (Math.Abs(_p.velocity.Length) < 1e-2)
                        _p.SetState(EntityState.AoeAttack, 6);
                    else
                        _p.SetState(EntityState.DirectedAttack, 6);
                    var entities = _p.worldObj.GetEntitiesWithinAaBbAndHeight(
                        _p.GetTranslatedAaBb().Scaled(_p.velocity.Normalized.Divide(4, 2), 1.05), _p.Position.z);
                    entities.Where(_e => _e is EntityLiving el && !el.state.Equals(EntityState.Dead) && _e != _p)
                        .Cast<EntityLiving>().ToList()
                        .ForEach(_e => _e.DealDamage(30));
                }

                _p.spells.InterruptCast();
            }
        };

        public static Action<Player> spawnBox = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
            {
                var box = new Box(_p.worldObj, _p.Position);
                box.velocity += _p.velocity.Normalized;
                _p.worldObj.SpawnEntity(box);
            }
        };

        public static Action<Player> spawnPile = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
                _p.worldObj.SpawnEntity(new BonePile(_p.worldObj, _p.Position));
        };

        public static Action<Player> spawnStreetlight = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
                _p.worldObj.SpawnEntity(new Streetlight(_p.worldObj, _p.Position));
        };

        public static Action<Player> spawnPotion = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
                _p.worldObj.SpawnEntity(new Potion(_p.worldObj, _p.Position));
        };

        public static void Register()
        {
            Bind(Key.W, moveUp, "move.up");
            Bind(Key.A, moveLeft, "move.left");
            Bind(Key.S, moveDown, "move.down");
            Bind(Key.D, moveRight, "move.right");
            BindClick(Key.I, openInventory, "gui.inv");
            BindClick(Key.T, openChat, "gui.chat");
            BindClick(Key.Escape, openIngameMenu, "gui.menu");
            BindClick(Key.Grave, exit, "exit");
            BindClick(Key.Space, attack, "world.attack");

            BindClick(Key.G, spawnSlime, "world.spawn.slime");
            BindClick(Key.B, spawnBat, "world.spawn.bat");

            BindClick(Key.Keypad1, spawnBox, "world.spawn.Box");
            BindClick(Key.Keypad2, spawnPile, "world.spawn.Pile");
            BindClick(Key.Keypad3, spawnStreetlight, "world.spawn.Streetlight");
            BindClick(Key.Keypad4, spawnPotion, "world.spawn.Potion");

            BindClick(Key.Keypad0, saveWorld, "world.save.world");
            BindClick(Key.Enter, setBlock, "world.set.block");

            BindClick(Key.Number1, spellAir, "spell.air");
            BindClick(Key.Number2, spellEarth, "spell.earth");
            BindClick(Key.Number3, spellWater, "spell.water");
            BindClick(Key.Number4, spellFire, "spell.fire");
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
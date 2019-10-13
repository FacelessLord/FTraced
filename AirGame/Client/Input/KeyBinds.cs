using System;
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Graphic.Gui;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Entities.Items;
using GlLib.Common.Map;
using GlLib.Common.SpellCastSystem;
using GlLib.Utils.Math;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class KeyBinds
    {
        public static Dictionary<Key, Func<EntityPlayer, bool>> binds = new Dictionary<Key, Func<EntityPlayer, bool>>();
        public static Dictionary<Key, Func<EntityPlayer, bool>> clickBinds = new Dictionary<Key, Func<EntityPlayer, bool>>();

        public static Dictionary<Func<EntityPlayer, bool>, string> delegateNames =
            new Dictionary<Func<EntityPlayer, bool>, string>();

        public static Func<EntityPlayer, bool> moveLeft = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.direction = Direction.Left;
            _p.SetState(EntityState.Walk, 3);
            _p.velocity += new PlanarVector(-_p.accelerationValue);
            return true;
        };

        public static Func<EntityPlayer, bool> moveUp = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.SetState(EntityState.Walk, 3);
            _p.velocity += new PlanarVector(0, -_p.accelerationValue);
            return true;
        };

        public static Func<EntityPlayer, bool> setBlock = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            var chunkX = _p.Position.Ix / 16;
            var chunkY = _p.Position.Iy / 16;

            var blockX = _p.Position.Ix % 16;
            var blockY = _p.Position.Iy % 16;

            _p.worldObj[chunkX, chunkY][blockX, blockY] = Proxy.GetClient().entityPlayer.Brush;
            return true;
        };

        public static Func<EntityPlayer, bool> saveWorld = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            WorldManager.SaveChunks(_p.worldObj);
            return true;
        };


        public static Func<EntityPlayer, bool> moveRight = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.SetState(EntityState.Walk, 3);
            _p.direction = Direction.Right;
            _p.velocity += new PlanarVector(_p.accelerationValue);
            return true;
        };

        public static Func<EntityPlayer, bool> moveDown = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.SetState(EntityState.Walk, 3);
            _p.velocity += new PlanarVector(0, _p.accelerationValue);
            return true;
        };

        public static Func<EntityPlayer, bool> openInventory = _p =>
        {
            if (!Proxy.GetWindow().serverStarted) return false;
            Proxy.GetWindow().TryOpenGui(new PlayerInventoryGui(_p));
            return true;
        };

        public static Func<EntityPlayer, bool> openIngameMenu = _p =>
        {
            if (!Proxy.GetWindow().serverStarted) return false;
            Proxy.GetWindow().TryOpenGui(new GuiIngameMenu(), true);
            return true;
        };

        public static Func<EntityPlayer, bool> openChat = _p =>
        {
            if (!Proxy.GetWindow().serverStarted) return false;
            Proxy.GetWindow().TryOpenGui(new GuiChat());
            return true;
        };

        public static Func<EntityPlayer, bool> spawnSlime = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.worldObj.SpawnEntity(new EntitySlime(_p.worldObj, _p.Position));
            return true;
        };

        public static Func<EntityPlayer, bool> spawnBat = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.worldObj.SpawnEntity(new EntityBat(_p.worldObj, _p.Position));
            return true;
        };

        public static Func<EntityPlayer, bool> exit = _p =>
        {
            if (Proxy.GetWindow().CanMovementBeHandled())
                Proxy.Exit = true;
            return true;
        };

        public static Func<EntityPlayer, bool> spellFire = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.spells.OnUpdate(ElementType.Fire);
            return true;
        };

        public static Func<EntityPlayer, bool> spellAir = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.spells.OnUpdate(ElementType.Air);
            return true;
        };

        public static Func<EntityPlayer, bool> spellEarth = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.spells.OnUpdate(ElementType.Earth);
            return true;
        };

        public static Func<EntityPlayer, bool> spellWater = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.spells.OnUpdate(ElementType.Water);
            return true;
        };


        public static Func<EntityPlayer, bool> attack = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            if (!(_p.state is EntityState.AttackInterrupted))
            {
                if (Math.Abs(_p.velocity.Length) < 1e-2)
                    _p.SetState(EntityState.AoeAttack, 6);
                else
                    _p.SetState(EntityState.DirectedAttack, 6);
                var entities = _p.worldObj.GetEntitiesWithinAaBb(
                    _p.GetTranslatedAaBb().Scaled(_p.velocity.Normalized.Divide(4, 1), 1.05f));
                entities.Where(_e => _e is EntityLiving el && !el.state.Equals(EntityState.Dead) && _e != _p)
                    .Cast<EntityLiving>().ToList()
                    .ForEach(_e => _e.DealDamage(30));
            }

            _p.spells.InterruptCast();
            return true;
        };

        public static Func<EntityPlayer, bool> spawnBox = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            var box = new Box(_p.worldObj, _p.Position);
            box.velocity += _p.velocity.Normalized;
            _p.worldObj.SpawnEntity(box);
            return true;
        };

        public static Func<EntityPlayer, bool> spawnPile = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.worldObj.SpawnEntity(new BonePile(_p.worldObj, _p.Position));
            return true;
        };

        public static Func<EntityPlayer, bool> spawnStreetlight = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.worldObj.SpawnEntity(new Streetlight(_p.worldObj, _p.Position));
            return true;
        };

        public static Func<EntityPlayer, bool> spawnPotion = _p =>
        {
            if (!Proxy.GetWindow().CanMovementBeHandled()) return false;
            _p.worldObj.SpawnEntity(new Potion(_p.worldObj, _p.Position));
            return true;
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

            BindClick(Key.Z, spawnBox, "world.spawn.Box");
            BindClick(Key.X, spawnPile, "world.spawn.Pile");
            BindClick(Key.C, spawnStreetlight, "world.spawn.Streetlight");
            BindClick(Key.V, spawnPotion, "world.spawn.Potion");

            BindClick(Key.Keypad0, saveWorld, "world.save.world");
            BindClick(Key.Enter, setBlock, "world.set.block");

            BindClick(Key.Number1, spellAir, "spell.air");
            BindClick(Key.Number2, spellEarth, "spell.earth");
            BindClick(Key.Number3, spellWater, "spell.water");
            BindClick(Key.Number4, spellFire, "spell.fire");
        }

        public static void Bind(Key _key, Func<EntityPlayer, bool> _action, string _name)
        {
            binds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
            delegateNames.Add(_action, _name);
        }

        public static void BindClick(Key _key, Func<EntityPlayer, bool> _action, string _name)
        {
            clickBinds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
            delegateNames.Add(_action, _name);
        }

        public static void RebindClick(Key _key, Func<EntityPlayer, bool> _action)
        {
            clickBinds.Remove(clickBinds.Keys.Single(_k => clickBinds[_k] == _action));
            clickBinds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
        }

        public static void Rebind(Key _key, Func<EntityPlayer, bool> _action)
        {
            binds.Remove(binds.Keys.Single(_k => binds[_k] == _action));
            binds.Add(_key, _action);
            KeyboardHandler.RegisterKey(_key);
        }
    }
}
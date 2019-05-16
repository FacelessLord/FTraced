using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Common.SpellCastSystem;
using GlLib.Utils;
using System;
using System.Collections.Generic;
using System.Net.Json;

namespace GlLib.Common.Entities
{
    public class Player : EntityLiving
    {
        public double accelerationValue = 0.1;
        public PlayerData data;
        public PlayerInventory inventory = new PlayerInventory();
        public string nickname = "Player";
        public HashSet<string> usedBinds = new HashSet<string>();
        internal SpellSystem spells;

        public Player(string _nickname,
            World _world,
            RestrictedVector3D _position,
            bool _godMode, uint _health,
            ushort _armor) : base(_health, _armor, _world, _position)
        {
            nickname = _nickname;
            Initialization();
        }

        public Player()
        {
            Initialization();
        }


        private void Initialization()
        {

            SidedConsole.WriteLine("Setting Player Renderer");
            SetCustomRenderer(new PlayerRenderer());
            SidedConsole.WriteLine("Setting Player Inventory");
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.varia));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.apple));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.sword));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.armor));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.ring));

            spells = new SpellSystem(this);

        }

        public override string GetName()
        {
            return "entity.living.player";
        }

        public override void Update()
        {
            base.Update();
        }

        public override void LoadFromJsonObject(JsonObject _jsonObject)
        {
            base.LoadFromJsonObject(_jsonObject);
            if (_jsonObject is JsonObjectCollection collection)
            {
//                SidedConsole.WriteLine(collection.Select(_o => _o.ToString()).Aggregate("", (_a, _b) => _a + _b));
                nickname = ((JsonStringValue) collection[13]).Value;
                data = PlayerData.LoadFromNbt(NbtTag.FromString(((JsonStringValue) collection[14]).Value));
            }
        }

        public override JsonObject CreateJsonObject()
        {
            var obj = base.CreateJsonObject();
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonStringValue("nickName", nickname));
                if (data != null)
                {
                    var tag = new NbtTag();
                    data.SaveToNbt(tag);
                    collection.Add(new JsonStringValue("tag", tag.ToString()));
                }
            }

            return obj;
        }

        public void CheckVelocity()
        {
            if (Math.Abs(velocity.x) > maxVel.x) velocity.x *= maxVel.x / Math.Abs(velocity.x);
            if (Math.Abs(velocity.y) > maxVel.y) velocity.y *= maxVel.y / Math.Abs(velocity.y);
        }

        public override AxisAlignedBb GetAaBb()
        {
            return base.GetAaBb() + new PlanarVector(0, 1);
        }
    }
}
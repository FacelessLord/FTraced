using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Utils;
using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Common.SpellCastSystem;

namespace GlLib.Common.Entities
{
    public class Player : Entity
    {
        public PlayerData data;
        public double accelerationValue = 0.2;
        public string nickname = "Player";
        public HashSet<string> usedBinds = new HashSet<string>();
        public PlayerInventory inventory = new PlayerInventory();
        internal SpellSystem spells;

        public Player(string _nickname, World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            nickname = _nickname;
            SetCustomRenderer(new PlayerRenderer());
        }

        public Player(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            SetCustomRenderer(new PlayerRenderer());
        }

        public Player()
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
            return "entity.player";
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
                nickname = ((JsonStringValue) collection[7]).Value;
                data = PlayerData.LoadFromNbt(NbtTag.FromString(((JsonStringValue) collection[8]).Value));
            }
        }

        public override JsonObject CreateJsonObject()
        {
            JsonObject obj = base.CreateJsonObject();
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonStringValue("nickName", nickname));
                if (data != null)
                {
                    NbtTag tag = new NbtTag();
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
    }
}
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Client.Api.Renderers;
using GlLib.Client.Graphic.Gui;
using GlLib.Common.Api.Entity;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Io;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Common.SpellCastSystem;
using GlLib.Utils.Collections;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities
{
    public class Player : EntityLiving, IAttacker
    {
        public float accelerationValue = 0.05f;
        public ChatIo chatIo = new ChatIo();
        public PlayerData data;
        public EquipmentInventory equip = new EquipmentInventory();
        public PlayerInventory inventory = new PlayerInventory();

        public int money = 0;
        public string nickname = "Player";
        internal SpellSystem spells;
        public HashSet<string> usedBinds = new HashSet<string>();

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

        public TerrainBlock Brush { get; internal set; }

        public int AttackValue { get; set; }

        private void Initialization()
        {
            Brush = Proxy.GetRegistry().GetBlockFromId(0);
            SidedConsole.WriteLine("Setting Player Renderer");
            SetCustomRenderer(new AttackingLivingRenderer("player/dwarf"));
            SidedConsole.WriteLine("Setting Player Inventory");
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.varia));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.apple));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.dawnBlade));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.dawnArmor));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.dawnBoots));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.dawnShield));
            inventory.AddItemStack(new ItemStack(Proxy.GetRegistry().itemRegistry.ring));

            CanDie = false;
            spells = new SpellSystem(this);

            AaBb = new AxisAlignedBb(-0.4f, 0.1f, 0.4f, 0.8f);
        }

        public override void DealDamage(float _damage)
        {
            base.DealDamage(_damage);
            spells.InterruptCast();
        }

        public override string GetName()
        {
            return "entity.living.player";
        }

        public override void Update()
        {
            base.Update();
            if (state.Equals(EntityState.Dead))
                if (!(Proxy.GetWindow().guiFrame is ResurrectionGui) && this == Proxy.GetClient().player)
                {
                    Proxy.GetWindow().CloseGui();
                    Proxy.GetWindow().TryOpenGui(new ResurrectionGui());
                }
        }

        public override void Deserialize(JsonObject _jsonObject)
        {
            base.Deserialize(_jsonObject);
            if (_jsonObject is JsonObjectCollection collection)
                //                SidedConsole.WriteLine(collection.Select(_o => _o.ToString()).Aggregate("", (_a, _b) => _a + _b));
                nickname = ((JsonStringValue) collection[13]).Value;
//                data = new PlayerData().Deserialize(((JsonStringValue) collection[14]).Value);
        }

        public override JsonObject Serialize(string _objectName)
        {
            var obj = base.Serialize(_objectName);
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonStringValue("nickName", nickname));
                if (data != null)
                {
                    var tag = new NbtTag();
//                    data.SaveToNbt(tag);
                    collection.Add(new JsonStringValue("tag", tag.ToString()));
                }
            }

            return obj;
        }
    }
}
using System;
using System.Net.Json;
using GlLib.Common.API;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class EntityLiving : Entity, IJsonSerializable
    {
        public const ushort MaxArmor = 100;

        public EntityLiving(bool _godMode, uint _health,
            ushort _armor, World _world,
            RestrictedVector3D _position) : base(_world, _position)
        {
            Armor = _armor;
            Health = _health;
            GodMode = _godMode;

            IsTakingDamage = false;
        }

        public float Health { get; protected set; }

        public bool GodMode { get; protected set; }
        public bool IsTakingDamage { get; private set; } // TODO

        public ushort Armor
        {
            get => Armor;
            protected set
            {
                if (value < 100) Armor = value;
                else
                    throw new ArgumentException("Armor shouldn't be greater than 100.");
            }
        }

        public override JsonObject CreateJsonObject()
        {
            var obj = base.CreateJsonObject();
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonNumericValue("Armor", Armor ));
                collection.Add(new JsonNumericValue("Health", Health));
                collection.Add(new JsonStringValue("GodMode", GodMode + ""));
                collection.Add(new JsonStringValue("IsTakingDamage", IsTakingDamage + ""));
            }

            return obj;
        }

        public override void LoadFromJsonObject(JsonObject _jsonObject)
        {
            base.LoadFromJsonObject(_jsonObject);

            if (_jsonObject is JsonObjectCollection collection)
            {
                Armor =  (ushort) ((JsonNumericValue) collection[7]).Value;
                Health = (float) ((JsonNumericValue) collection[8]).Value;
                GodMode = ((JsonStringValue)collection[9]).Value == "True";
                IsTakingDamage = ((JsonStringValue)collection[10]).Value == "True";
            }
        }


        public void OnDealDamage(float _damage)
        {
            if (GodMode) return;

            SidedConsole.WriteLine("DealDamage!");

            var takenDamage = _damage * (Armor / MaxArmor);
            if (takenDamage >= Health)
            {
                isDead = true;
                Health = 0;
            }
            else
            {
                Health -= takenDamage;
            }
        }
        public override string GetName()
        {
            return "entity.entityLiving";
        }
    }
}
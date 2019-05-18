using System;
using System.Net.Json;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class EntityLiving : Entity
    {
        public const ushort MaxArmor = 100;

        private ushort _armor;

        public EntityLiving(uint _health,
            ushort _armor, World _world,
            RestrictedVector3D _position) : base(_world, _position)
        {
            Armor = _armor;
            Health = _health;
            MaxHealth = _health;

            DamageTimer = -1;
        }

        public EntityLiving()
        {
            Armor = 0;
            Health = 100;
            MaxHealth = 100;
            DamageTimer = -1;
        }

        public bool CanDie { get; set; } = true;

        public float Health { get; protected set; }
        public float MaxHealth { get; protected set; }
        public bool GodMode { get; protected set; }
        public int DamageTimer { get; private set; }

        public ushort Armor
        {
            get => _armor;
            protected set
            {
                if (value < 100) _armor = value;
                else
                    throw new ArgumentException("Armor shouldn't be greater than 100.");
            }
        }

        public void SetGodMode(bool _enable = true)
        {
            GodMode = _enable;
        }

        public override JsonObject CreateJsonObject()
        {
            var obj = base.CreateJsonObject();
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonNumericValue("Armor", Armor));
                collection.Add(new JsonNumericValue("Health", Health));
                collection.Add(new JsonNumericValue("MaxHealth", MaxHealth));
                collection.Add(new JsonStringValue("GodMode", GodMode + ""));
                collection.Add(new JsonNumericValue("IsTakingDamage", DamageTimer));
            }

            return obj;
        }

        public override void LoadFromJsonObject(JsonObject _jsonObject)
        {
            base.LoadFromJsonObject(_jsonObject);

            if (_jsonObject is JsonObjectCollection collection)
            {
                Armor = (ushort) ((JsonNumericValue) collection[8]).Value;
                Health = (float) ((JsonNumericValue) collection[9]).Value;
                MaxHealth = (float) ((JsonNumericValue) collection[10]).Value;
                GodMode = ((JsonStringValue) collection[11]).Value == "True";
                DamageTimer = (int) ((JsonNumericValue) collection[12]).Value;
            }
        }

        public override void Update()
        {
            base.Update();

            if (Health <= 0 && DamageTimer == 0 && CanDie)
                SetDead();
            if (DamageTimer > 0)
                DamageTimer--;
            if (DamageTimer < 0)
                DamageTimer++;

            if (state is EntityState.Dead && Health > 0) SetState(EntityState.Idle, -1, true);
            if (!(state is EntityState.Dead) && Health <= 0) SetState(EntityState.Dead, -1, true);
        }

        public void DealDamage(float _damage)
        {
            if (GodMode) return;
            if (!state.Equals(EntityState.Dead))
            {
                SetState(EntityState.AttackInterrupted, 3);
                var takenDamage = _damage * (1 - Armor / (float) MaxArmor);
                if (takenDamage >= Health)
                {
                    Health = 0;
                    DamageTimer = 2;
                    SetState(EntityState.Dead, -1);
//                    SidedConsole.WriteLine("Dead");
                }
                else
                {
                    DamageTimer = 2;
                    Health -= takenDamage;
                }

//            SidedConsole.WriteLine("Damage Dealt: " + takenDamage + "; " + Health);
            }
        }

        public void Heal(float _damage)
        {
            Health += _damage;
            DamageTimer = -2;
            if (Health > MaxHealth) Health = MaxHealth;
        }

//        public virtual void PerformAttack()
//        {
//            
//        }

        public override string GetName()
        {
            return "entity.entityLiving";
        }
    }
}
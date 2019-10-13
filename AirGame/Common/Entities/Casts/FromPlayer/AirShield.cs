using System;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Casts.FromPlayer
{
    public class AirShield : Entity
    {
        private const short BaseVelocity = 0;
        public float size = 1.1f;

        public Entity Caster { get; }

        public AirShield(World _world, Entity _caster, RestrictedVector3D _position, PlanarVector _velocity,
            uint _dieTime, int _damage)
            : base(_world, _position)
        {
            DieTime = _dieTime;
            Damage = _damage;
            Caster = _caster;
            velocity = _velocity * BaseVelocity;
            SetCustomRenderer(new AirShieldRenderer());
            AaBb = new AxisAlignedBb(-30, -30f, 30, 30f);
        }

        internal uint DieTime { get; }
        internal int Damage { get; }

        public override void Update()
        {
            base.Update();

            if (InternalTicks > DieTime)
                SetDead();

            size *= 1.1f;
        }

        public override void OnCollideWith(Entity _obj)
        {
            PlanarVector r = _obj.Position - Position;
            if (Caster != _obj && !(_obj is AirShield) && r.Length * r.Length <= size && r.Length * r.Length > 4)
            {
                var k = 8f;
                var F = k * r / (r.Length * r.Length * r.Length);
                if (F.Length > 0.5f)
                    F.Length = 0.5f;
                    _obj.velocity += F;
            }
        }
    }
}
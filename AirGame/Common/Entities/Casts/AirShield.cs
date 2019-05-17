using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class AirShield : Entity
    {
        private const short BaseVelocity = 0;
        private readonly long spawnTime;
        internal uint DieTime { get; }
        internal int Damage { get; }

        internal long InternalTime
            => DateTime.Now.Ticks - spawnTime;

        public AirShield(World _world, RestrictedVector3D _position, PlanarVector _velocity, uint _dieTime, int _damage)
            : base(_world, _position)
        {
            DieTime = _dieTime;
            Damage = _damage;
            spawnTime = DateTime.Now.Ticks;
            velocity = _velocity * BaseVelocity;
            SetCustomRenderer(new AirShieldRenderer());
        }

        public override AxisAlignedBb GetAaBb()
        {
            return new PlanarVector().ExpandBothTo(3,6);
        }

        public override void Update()
        {
            base.Update();

            if (InternalTime > DieTime)
                SetDead(true);
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (!(_obj is Player) && _obj is EntityLiving)
                (_obj as EntityLiving).velocity *= -1;
        }
    }
}

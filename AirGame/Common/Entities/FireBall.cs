using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class FireBall : Entity
    {
        private const short BaseVelocity = 2;
        private readonly long spawnTime;
        internal uint DieTime { get; }
        internal int Damage { get; }

        internal long InternalTime
            =>  DateTime.Now.Ticks - spawnTime;

        public FireBall(World _world, RestrictedVector3D _position, Direction direction, PlanarVector _velocity, uint _dieTime, int _damage)
            : base(_world, _position)
        {
            PlanarVector sightDirection;
            if (direction == Direction.Right)
                sightDirection = new PlanarVector(2, 0);
            else sightDirection = new PlanarVector(-2, 0);

            DieTime = _dieTime;
            Damage = _damage;
            spawnTime = DateTime.Now.Ticks;
            velocity = _velocity.Normalized == new PlanarVector(0, 0) ? sightDirection : _velocity.Normalized * BaseVelocity;
            SetCustomRenderer(new FireBallRenderer(_velocity.Normalized, direction));
        }

        public override AxisAlignedBb GetAaBb()
        {
            return Position.ToPlanar().ExpandBothTo(2, 1.5) + -new PlanarVector(0, -2);
        }

        public override AxisAlignedBb GetAaBb()
        {
            return Position.ToPlanar().ExpandBothTo(2, 1.5) + -new PlanarVector(0, -2);
        }
        public override void Update()
        {
            base.Update();

            //if (InternalTime > DieTime)
            //    SetDead(true);
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (_obj is EntityLiving)
                (_obj as EntityLiving).DealDamage(Damage);
        }
    }
}

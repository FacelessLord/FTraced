using System;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Casts.FromPlayer
{
    public class FireBall : Entity
    {
        private const short BaseVelocity = 2;

        public FireBall(World _world, RestrictedVector3D _position, Direction direction, PlanarVector _velocity,
            uint _dieTime, int _damage)
            : base(_world, _position)
        {
            isAffectedByFriction = false;
            PlanarVector sightDirection;
            if (direction == Direction.Right)
                sightDirection = new PlanarVector(1);
            else sightDirection = new PlanarVector(-1);

            DieTime = _dieTime;
            Damage = _damage;

            velocity = _velocity.Normalized == new PlanarVector(0) ? sightDirection : _velocity.Normalized;
            SetCustomRenderer(new FireBallRenderer(_velocity.Normalized, direction));
            AaBb = new AxisAlignedBb(-0.5f, -1f, 0.5f, 1f);
        }

        internal uint DieTime { get; }
        internal int Damage { get; }

        public override void Update()
        {
            base.Update();

            if (Math.Abs(velocity.Length) < 1e-3)
                SetDead();

            if (InternalTicks > DieTime)
                SetDead();
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (_obj is EntityLiving && !(_obj is Player))
                (_obj as EntityLiving).DealDamage(Damage);
        }
    }
}
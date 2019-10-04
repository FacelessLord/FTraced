using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Casts.FromPlayer
{
    public class AirBlow : Entity
    {
        //register this
        private readonly PlanarVector _baseVelocity = new PlanarVector(0, 0);


        public AirBlow(World _world, RestrictedVector3D _position, PlanarVector _velocity, uint _dieTime, int _damage)
            : base(_world, _position)
        {
            DieTime = _dieTime;
            Damage = _damage;
            position = _position + _velocity * 1 / _velocity.Length * 1.5f;
            velocity = _baseVelocity;
            SetCustomRenderer(new AirBlowRenderer());
        }

        internal uint DieTime { get; }
        internal int Damage { get; }

        public override void Update()
        {
            base.Update();

            if (InternalTicks > DieTime)
                SetDead(true);
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (!(_obj is Player) && _obj is EntityLiving)
                (_obj as EntityLiving).velocity = -(_obj as EntityLiving).velocity + new PlanarVector(1, 0);
        }
    }
}
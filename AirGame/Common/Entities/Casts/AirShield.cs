using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class AirShield : Entity
    {
        private const short BaseVelocity = 0;


        public AirShield(World _world, RestrictedVector3D _position, PlanarVector _velocity, uint _dieTime, int _damage)
            : base(_world, _position)
        {
            DieTime = _dieTime;
            Damage = _damage;
            velocity = _velocity * BaseVelocity;
            SetCustomRenderer(new AirShieldRenderer());
            AaBb = new AxisAlignedBb(-3, -6, 3, 6);
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
                (_obj as EntityLiving).velocity *= -1;
        }
    }
}
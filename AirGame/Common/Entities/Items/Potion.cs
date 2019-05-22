using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities.Items
{
    internal class Potion : Entity
    {
        public Potion()
        {
            SetCustomRenderer(new CoinRenderer());
            Initialize();
        }

        public Potion(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            SetCustomRenderer(new PotionRenderer());
            Initialize();
        }

        public Potion(World _world, RestrictedVector3D _position, PlanarVector _velocity) : base(_world, _position)
        {
            velocity = _velocity;
            SetCustomRenderer(new PotionRenderer());
            Initialize();
        }

        public void Initialize()
        {
            AaBb = new AxisAlignedBb(-0.15, -0.25, 0.15, 0.25);
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (_obj is Player p)
            {
                p.Heal(10);
                SetDead();
            }
        }

        public override string GetName()
        {
            return "entity.coin";
        }
    }
}
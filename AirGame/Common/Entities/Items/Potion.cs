using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;
using GlLib.Utils.Math;

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
            AaBb = new AxisAlignedBb(-0.15f, -0.25f, 0.15f, 0.25f);
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
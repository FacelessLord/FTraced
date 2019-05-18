using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Coin : Entity
    {
        public Coin()
        {
            SetCustomRenderer(new CoinRenderer());
            Initialize();
        }

        public Coin(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            SetCustomRenderer(new CoinRenderer());
            Initialize();
        }

        public Coin(World _world, RestrictedVector3D _position, PlanarVector _velocity) : base(_world, _position)
        {
            velocity = _velocity;
            SetCustomRenderer(new CoinRenderer());
            Initialize();
        }

        public void Initialize()
        {
            AaBb = new AxisAlignedBb(-0.15, 0.05, 0.15, 0.25);
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (_obj is Player p)
            {
                p.money += 10;
                SetDead();
            }
        }

        public override string GetName()
        {
            return "entity.coin";
        }
    }
}
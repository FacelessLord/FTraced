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
        }
        public Coin(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            SetCustomRenderer(new CoinRenderer());
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
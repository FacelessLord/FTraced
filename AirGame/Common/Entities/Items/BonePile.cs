using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class BonePile : EntityLiving
    {
        public BonePile()
        {
            MaxHealth = 1;
            Health = 1;
            SetCustomRenderer(new BonePileRenderer());
        }

        public BonePile(World _world, RestrictedVector3D _position, uint _health = 1,
            ushort _armor = 1) : base(_health, _armor, _world, _position)
        {
            SetCustomRenderer(new BonePileRenderer());
        }

        public override void OnDead()
        {
            if (Proxy.GetWindow().serverStarted)
                worldObj.SpawnEntity(
                    new Coin(worldObj, Position,
                        PlanarVector.GetRandom(0.2)));
        }

        public override string GetName()
        {
            return "entity.bone_pile";
        }
    }
}
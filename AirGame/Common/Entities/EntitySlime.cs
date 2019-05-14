using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class EntitySlime : EntityLiving
    {
        public EntitySlime()
        {
            SetCustomRenderer(new SlimeRenderer());
        }
        public EntitySlime( World _world, RestrictedVector3D _position) : base(100, 0,_world, _position)
        {
            SetCustomRenderer(new SlimeRenderer());
        }

        public override string GetName()
        {
            return "entity.living.slime";
        }
    }
}
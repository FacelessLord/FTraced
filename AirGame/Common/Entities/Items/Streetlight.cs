using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Items
{
    public class Streetlight : Entity
    {
        public Streetlight()
        {
            Initialize();
        }

        public Streetlight(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            Initialize();
        }

        private void Initialize()
        {
            noClip = true;
            SetCustomRenderer(new StreetlightRenderer());
        }

        public override string GetName()
        {
            return "entity.streetlight";
        }
    }
}
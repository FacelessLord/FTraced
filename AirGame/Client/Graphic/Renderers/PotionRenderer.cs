using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.Graphic.Renderers
{
    internal class PotionRenderer : EntityRenderer
    {
        public LinearSprite potionSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(SimpleStructPath + "HealthPotion.png", 1, 1);
            potionSprite = new LinearSprite(layout, 1, 1).SetFrozen();
            var box = _e.AaBb;
            potionSprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            potionSprite.Render();
        }
    }
}
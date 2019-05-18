using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.Graphic.Renderers
{
    internal class PotionRenderer : EntityRenderer
    {
        public ISprite coinSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(SimpleStructPath + "HealthPotion.png", 1, 1);
            coinSprite = new LinearSprite(layout, 1, 1).SetFrozen();
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            coinSprite.Render();
        }
    }
}
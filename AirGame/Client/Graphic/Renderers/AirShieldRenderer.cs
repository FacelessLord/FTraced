using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.Graphic.Renderers
{
    internal class AirShieldRenderer : EntityRenderer
    {
        private ISprite _sprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(@"air_shield.png", 4, 1);
            _sprite = new LinearSprite(layout, 4, 6);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
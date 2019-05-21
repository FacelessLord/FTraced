using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.Graphic.Renderers
{
    internal class AirBlowRenderer : EntityRenderer
    {
        private LinearSprite _sprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(@"air_blow.png", 3, 1);
            _sprite = new LinearSprite(layout, 3, 10);
            var box = _e.AaBb;
            _sprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
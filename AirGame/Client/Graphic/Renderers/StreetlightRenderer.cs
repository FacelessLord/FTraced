using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;

namespace GlLib.Client.Graphic.Renderers
{
    internal class StreetlightRenderer : EntityRenderer
    {
        private LinearSprite _sprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(SimpleStructPath + @"Streetlight.png", 1, 1);
            _sprite = new LinearSprite(layout, 1, 1).SetFrozen();
            var box = _e.AaBb;
            _sprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
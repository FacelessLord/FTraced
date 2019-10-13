using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;

namespace GlLib.Client.Graphic.Renderers
{
    internal class AirShieldRenderer : EntityRenderer
    {
        private LinearSprite _sprite;

        protected override void Setup(Entity _e)
        {
            var layout = new TextureLayout(Textures.airShield, 4, 1);
            _sprite = new LinearSprite(layout, 4, 6);
            var box = _e.AaBb;
            _sprite.Scale((float) box.Width*2, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
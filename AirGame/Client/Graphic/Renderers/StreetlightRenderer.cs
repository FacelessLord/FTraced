using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils.Math;

namespace GlLib.Client.Graphic.Renderers
{
    internal class StreetlightRenderer : EntityRenderer
    {
        private LinearSprite _sprite;

        protected override void Setup(Entity _e)
        {
            var layout = new TextureLayout(Textures.streetLight, 1, 1);
            _sprite = new LinearSprite(layout, 1).SetFrozen();
            var box = _e.AaBb;
            _sprite.Scale(box.Width, box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Common.Entities.Casts.FromPlayer;
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
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            float size = (_e as AirShield).size;
            _sprite.Scale(6f * size, 4.5f * size);
            _sprite.Render();
            _sprite.Rescale();
        }
    }
}
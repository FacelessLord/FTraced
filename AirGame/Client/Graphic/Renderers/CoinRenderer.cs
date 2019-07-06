using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK;

namespace GlLib.Client.Graphic.Renderers
{
    public class CoinRenderer : EntityRenderer
    {
        public LinearSprite coinSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(Textures.coin, 8, 1);
            coinSprite = new LinearSprite(layout, 8, 18);
//            coinSprite.Translate(new PlanarVector(-8, -8));
//            coinSprite.Scale(new Vector3(0.5f, 0.5f, 1));
            var box = _e.AaBb;
            coinSprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            coinSprite.Render();
        }
    }
}
using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class CoinRenderer : EntityRenderer
    {
        public LinearSprite coinSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(SimpleStructPath + "Coins.png", 8, 1);
            coinSprite = new LinearSprite(layout, 8, 18);
            coinSprite.MoveSpriteTo(new PlanarVector(-8,-8));
            coinSprite.SetSize(new Vector3(0.5f,0.5f,1));
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            coinSprite.Render();
        }
    }
}
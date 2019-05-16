using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class CoinRenderer : EntityRenderer
    {
        public ISprite coinSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout("coin.png", 1, 1);
            coinSprite = new LinearSprite(layout, 1, 20).SetFrozen();
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            coinSprite.Render();
            GL.PopMatrix();
        }
    }
}
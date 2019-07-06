using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class CastleRenderer : IBlockRenderer
    {
        public Sprite castleSprite;

        public CastleRenderer()
        {
            var layout = new TextureLayout(Textures.castle, 1, 1);
            castleSprite = new LinearSprite(layout, 1, 20).SetFrozen();
        }

        public void Render(World _world, int _x, int _y)
        {
            GL.Translate(-3*Chunk.BlockWidth,-Chunk.BlockHeight,0);
            GL.Scale(8,2,1);
            castleSprite.Render();
        }
    }
}
using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class CastleRenderer : IBlockRenderer
    {
        public ISprite castleSprite;
        protected const string SimpleStructPath = @"simple_structs/";

        public CastleRenderer()
        {
            var layout = new TextureLayout(SimpleStructPath + "Castle.png", 1, 1);
            castleSprite = new LinearSprite(layout, 1, 20).SetFrozen();
        }

        public void Render(World _world, int _x, int _y)
        {
            GL.Translate(-3*Chunk.BlockWidth,-Chunk.BlockHeight,0);
            GL.Scale(1,0.5d,1);
            castleSprite.Render();
        }
    }
}
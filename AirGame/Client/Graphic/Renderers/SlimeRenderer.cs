using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class SlimeRenderer : EntityRenderer
    {
        public ISprite slimeSprite;

        public override void Setup(Entity _p)
        {
            var layout = new TextureLayout("slime/slime_waiting.png", 7, 1);
            slimeSprite = new LinearSprite(layout, 22, 20);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            slimeSprite.Render();
            GL.PopMatrix();
        }
    }
}
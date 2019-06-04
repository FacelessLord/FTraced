using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class BoxRenderer : EntityRenderer
    {
        public LinearSprite boxSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(Textures.box, 1, 1);
            boxSprite = new LinearSprite(layout, 1, 20).SetFrozen();
//            var box = _e.AaBb;
//            boxSprite.Scale((float) box.Width, (float) box.Height*2);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            boxSprite.Render();
            GL.PopMatrix();
        }
    }
}
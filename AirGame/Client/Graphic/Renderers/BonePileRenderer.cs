using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class BonePileRenderer : EntityRenderer
    {
        public LinearSprite boneSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(Textures.bonePile, 1, 1);
            boneSprite = new LinearSprite(layout, 1, 20).SetFrozen();
            var box = _e.AaBb;
            boneSprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            boneSprite.Render();
            GL.PopMatrix();
        }
    }
}
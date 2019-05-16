using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class BonePileRenderer : EntityRenderer
    {
        public ISprite boneSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout("simple structs/Relics.png", 1, 1);
            boneSprite = new LinearSprite(layout, 1, 20).SetFrozen();
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            boneSprite.Render();
            GL.PopMatrix();
        }
    }
}
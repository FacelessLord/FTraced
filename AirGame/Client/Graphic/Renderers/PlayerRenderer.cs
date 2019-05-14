using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class PlayerRenderer : EntityRenderer
    {
        public ISprite playerSprite;

        public override void Setup(Entity _p)
        {
            var layout = new TextureLayout("player_sprite.png", 16, 4);
            playerSprite = new LinearSprite(layout, 22, 1);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            playerSprite.Render();
            GL.PopMatrix();
        }
    }
}
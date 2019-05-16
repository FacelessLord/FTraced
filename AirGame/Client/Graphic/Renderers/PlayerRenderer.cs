using GlLib.Client.API;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class PlayerRenderer : EntityRenderer
    {
        public ISprite idleSprite;
        public ISprite walkSprite;
        public ISprite attackSprite;

        public override void Setup(Entity _p)
        {
            var idle = new TextureLayout("dwarf.png", 0, 0, 38 * 5, 32, 5, 1);
            var walk = new TextureLayout("dwarf.png", 0, 32, 38 * 8, 32 * 2, 8, 1);
            var attack = new TextureLayout("dwarf.png", 0, 32 * 4 + 2, 38 * 2, 32 * 5, 2, 1);

            idleSprite = new LinearSprite(idle, 5, 6);
            walkSprite = new LinearSprite(walk, 8, 6);
            attackSprite = new LinearSprite(attack, 2, 6);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            switch (_e.state)
            {
                case (EntityState.Idle):
                    idleSprite.Render();
                    break;
                case (EntityState.Walk):
                    walkSprite.Render();
                    break;
                case (EntityState.Attack):
                    attackSprite.Render();
                    break;
            }

            GL.PopMatrix();
        }
    }
}
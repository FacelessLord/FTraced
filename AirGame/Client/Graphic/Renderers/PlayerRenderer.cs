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
        public ISprite aoeAttackSprite;
        public ISprite directedAttackSprite;

        public override void Setup(Entity _p)
        {
            var idle = new TextureLayout("dwarf.png", 0, 0, 38 * 5, 32, 5, 1);
            var walk = new TextureLayout("dwarf.png", 0, 32, 38 * 8, 32 * 2, 8, 1);
            var aoeAttack = new TextureLayout("dwarf.png", 0, 32 * 4 + 2, 38 * 2, 32 * 5, 2, 1);
            var dirAttack = new TextureLayout("dwarf.png", 0, 32 * 3 + 2, 38 * 6, 32 * 4, 6, 1);

            idleSprite = new LinearSprite(idle, 5, 12);
            walkSprite = new LinearSprite(walk, 8, 12);
            aoeAttackSprite = new LinearSprite(aoeAttack, 2, 12);
            directedAttackSprite = new LinearSprite(dirAttack, 6, 12);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {

            switch (_e.state)
            {
                case (EntityState.Idle):
                    idleSprite.Render();
                    break;
                case (EntityState.Walk):
                    walkSprite.Render();
                    break;
                case (EntityState.AoeAttack):
                    aoeAttackSprite.Render();
                    break;
                case (EntityState.DirectedAttack):
                    directedAttackSprite.Render();
                    break;
            }
        }
    }
}
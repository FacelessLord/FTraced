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
        public LinearSprite idleSprite;
        public LinearSprite walkSprite;
        public LinearSprite aoeAttackSprite;
        public LinearSprite directedAttackSprite;
        public LinearSprite interruptedAttackSprite;
        public LinearSprite deathSprite;

        public override void Setup(Entity _p)
        {
            var idle = new TextureLayout("dwarf.png", 0, 0, 38 * 5, 32, 5, 1);
            var walk = new TextureLayout("dwarf.png", 0, 32, 38 * 8, 32 * 2, 8, 1);
            var aoeAttack = new TextureLayout("dwarf.png", 0, 32 * 4 + 2, 38 * 2, 32 * 5, 2, 1);
            var dirAttack = new TextureLayout("dwarf.png", 0, 32 * 3 + 2, 38 * 6, 32 * 4, 6, 1);
            var interrupt = new TextureLayout("dwarf.png", 0, 32 * 6 + 2, 38 * 4, 32 * 7, 4, 1);
            var death = new TextureLayout("dwarf.png", 0, 32 * 7 + 2, 38 * 7, 32 * 8, 7, 1);

            idleSprite = new LinearSprite(idle, 5, 12);
            walkSprite = new LinearSprite(walk, 8, 12);
            aoeAttackSprite = new LinearSprite(aoeAttack, 2, 12);
            directedAttackSprite = new LinearSprite(dirAttack, 6, 12);
            interruptedAttackSprite = new LinearSprite(interrupt, 4, 12);
            deathSprite = new LinearSprite(death, 7, 12).SetNoRepeat();
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
                case (EntityState.AttackInterrupted):
                    interruptedAttackSprite.Render();
                    break;
                case (EntityState.Dead):
                    deathSprite.Render();
                    break;
            }

            if (!_e.state.Equals(EntityState.Dead) && deathSprite.FullFrameCount != 0)
            {
                deathSprite.Reset();
            }

            //attack box render
//            GuiUtils.RenderAaBb(_e.GetAaBb().Scaled(_e.velocity.Normalized.Divide(4, 2), 1.05), Chunk.BlockWidth,
//                Chunk.BlockHeight);
        }
    }
}
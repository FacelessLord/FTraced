using System;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class SlimeRenderer : EntityRenderer
    {
        protected LinearSprite idleSprite;
        protected LinearSprite walkSprite;

        public SlimeRenderer() : base()
        {

        }

        public override void Setup(Entity _p)
        {
            var idle = new TextureLayout("slime/slime_idle.png", 10, 1);
            var walk = new TextureLayout("slime/slime_waiting.png", 7, 1);

            idleSprite = new LinearSprite(idle, 10, 30);
            walkSprite = new LinearSprite(walk, 7, 30);

            var s = _p as EntitySlime;
            
            idleSprite.SetColor(s.color);
            walkSprite.SetColor(s.color);
            var box = _p.AaBb;
            idleSprite.Scale((float) box.Width*2, (float) box.Height);
            walkSprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            
            switch (_e.state)
            {
                case (EntityState.Dead):
                case (EntityState.Idle):
                    idleSprite.Render();
                    break;
                case (EntityState.Walk):
                case (EntityState.DirectedAttack):
                case (EntityState.AoeAttack):
                case (EntityState.AttackInterrupted):
                    walkSprite.Render();
                    break;
            }
        }
    }
}
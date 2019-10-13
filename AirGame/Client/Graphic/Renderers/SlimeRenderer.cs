using System;
using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class SlimeRenderer : EntityRenderer
    {
        protected LinearSprite idleSprite;
        protected LinearSprite walkSprite;
        protected LinearSprite deathSprite;

        protected override void Setup(Entity _p)
        {
            var idle = new TextureLayout(Textures.slimeIdle, 10, 1);
            var death = new TextureLayout(Textures.slimeDeath, 10, 1);
            var walk = new TextureLayout(Textures.slimeWalk, 10, 1);

            idleSprite = new LinearSprite(idle, 10, 30);
            deathSprite = new LinearSprite(death, 10, 15)
                .SetNoRepeat();
            walkSprite = new LinearSprite(walk, 7, 5);

            var s = _p as EntitySlime;

            idleSprite.SetColor(s.color);
            walkSprite.SetColor(s.color);
            deathSprite.SetColor(s.color);

            var box = _p.AaBb;
            idleSprite.Scale(box.Width * 2, box.Height);
            deathSprite.Scale(box.Width * 2, box.Height);
            walkSprite.Scale(box.Width * 2, box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            if (_e.velocity.x > 0)
            {
                GL.Rotate(180, 0, 1, 0);
            }

            switch (_e.state)
            {
                case (EntityState.Dead):
                    deathSprite.Render();
                    break;
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

            GL.PopMatrix();
        }
    }
}
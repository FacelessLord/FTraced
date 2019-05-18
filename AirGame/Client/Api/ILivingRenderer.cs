using System;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API
{
    public class AttackingLivingRenderer : EntityRenderer
    {
        public string entityName;

        public ISprite idleSprite;
        public ISprite walkSprite;
        public ISprite aoeAttackSprite;
        public ISprite directedAttackSprite;
        public ISprite interruptedAttackSprite;
        public ISprite deathSprite;

        public AttackingLivingRenderer(string _entityName)
        {
            entityName = _entityName;
        }

        public override void Setup(Entity _e)
        {
            Texture idleTexture = Vertexer.LoadTexture(entityName + "_idle.png");
            Texture walkTexture = Vertexer.LoadTexture(entityName + "_walk.png");
            Texture aoeAttackTexture = Vertexer.LoadTexture(entityName + "_aoe_attack.png");
            Texture directedAttackTexture = Vertexer.LoadTexture(entityName + "_directed_attack.png");
            Texture interruptedAttackTexture = Vertexer.LoadTexture(entityName + "_interrupted.png");
            Texture deathTexture = Vertexer.LoadTexture(entityName + "_death.png");
            
            var idle = new TextureLayout(idleTexture, idleTexture.width / idleTexture.height, 1);
            var walk = new TextureLayout(walkTexture, walkTexture.width / walkTexture.height, 1);
            var aoeAttack = new TextureLayout(aoeAttackTexture, aoeAttackTexture.width / aoeAttackTexture.height, 1);
            var dirAttack = new TextureLayout(directedAttackTexture,
                directedAttackTexture.width / directedAttackTexture.height, 1);
            var interrupt = new TextureLayout(interruptedAttackTexture,
                interruptedAttackTexture.width / interruptedAttackTexture.height, 1);
            var death = new TextureLayout(deathTexture, deathTexture.width / deathTexture.height, 1);

            idleSprite = new LinearSprite(idle, idle.layout.countX, 12);
            walkSprite = new LinearSprite(walk, walk.layout.countX, 6);
            aoeAttackSprite = new LinearSprite(aoeAttack, aoeAttack.layout.countX, 6);
            directedAttackSprite = new LinearSprite(dirAttack, dirAttack.layout.countX, 6);
            interruptedAttackSprite = new LinearSprite(interrupt, interrupt.layout.countX, 6);
            deathSprite = new LinearSprite(death, death.layout.countX, 6).SetNoRepeat();
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            GL.Translate(0, _e.AaBb.Height/2,0);
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

            if (deathSprite is LinearSprite ls)
                if (!_e.state.Equals(EntityState.Dead) && ls.FullFrameCount != 0)
                {
                    ls.Reset();
                }
            GL.PopMatrix();
        }
    }
}
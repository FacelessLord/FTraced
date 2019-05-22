using System.Collections.Generic;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API
{
    public class AttackingLivingRenderer : EntityRenderer
    {
        public Dictionary<EntityState, LinearSprite> sprites = new Dictionary<EntityState, LinearSprite>();
        public string entityName;

        public AttackingLivingRenderer(string _entityName)
        {
            entityName = _entityName;
        }

        public override void Setup(Entity _e)
        {
            for (var i = EntityState.Idle; i <= EntityState.Dead; i++)
            {
                var texture = Vertexer.LoadTexture(entityName + "_" + i.ToString().ToLower() + ".png");
                var layout = new TextureLayout(texture, texture.width / texture.height, 1);
                var sprite = new LinearSprite(layout, layout.layout.countX, 6);
                
                var box = _e.AaBb;
                sprite.Scale((float) box.Width * 1.5f, (float) box.Height * 1.5f);
                sprites.Add(i, sprite);
            }

//            var idleTexture = Vertexer.LoadTexture(entityName + "_idle.png");
//            var walkTexture = Vertexer.LoadTexture(entityName + "_walk.png");
//            var aoeAttackTexture = Vertexer.LoadTexture(entityName + "_aoe_attack.png");
//            var directedAttackTexture = Vertexer.LoadTexture(entityName + "_directed_attack.png");
//            var interruptedAttackTexture = Vertexer.LoadTexture(entityName + "_interrupted.png");
//            var deathTexture = Vertexer.LoadTexture(entityName + "_death.png");
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            GL.Translate(0, _e.AaBb.Height / 2, 0);
            sprites[_e.state].Render();
            if (!_e.state.Equals(EntityState.Dead) && sprites[EntityState.Dead].FullFrameCount != 0)
                sprites[EntityState.Dead].Reset();
            GL.PopMatrix();
        }
    }
}
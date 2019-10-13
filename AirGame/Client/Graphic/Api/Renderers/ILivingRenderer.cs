using System;
using System.Collections.Generic;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK.Graphics.OpenGL;
using static GlLib.Common.Entities.EntityState;

namespace GlLib.Client.Api.Renderers
{
    public class AttackingLivingRenderer : EntityRenderer
    {
        public Dictionary<EntityState, LinearSprite> sprites = new Dictionary<EntityState, LinearSprite>();
        public string entityName;

        public AttackingLivingRenderer(string _entityName)
        {
            entityName = _entityName;
        }

        protected override void Setup(Entity _e)
        {
            for (var i = Idle; i <= Dead; i++)
            {
                var texture = Vertexer.LoadTexture(entityName + "_" + i.ToString().ToLower() + ".png");
                var layout = new TextureLayout(texture, texture.width / texture.height, 1);
                var sprite = new LinearSprite(layout, layout.layout.countX, 6);

                var box = _e.AaBb;
                sprite.Scale((float)box.Width * 1.5f, (float)box.Height * 1.5f);
                sprites.Add(i, sprite);
            }

            sprites[Dead].SetNoRepeat();
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            GL.Translate(0, _e.AaBb.Height / 2, 0);
            sprites[_e.state].Render();
            if (!(_e.state is Dead) && sprites[Dead].frozen)
                sprites[Dead].Reset();
            GL.PopMatrix();
        }
    }
    
    public class SimpleAttackingLivingRenderer : EntityRenderer
    {
        public Dictionary<EntityState, LinearSprite> sprites = new Dictionary<EntityState, LinearSprite>();
        public string entityName;
        public Action<Entity, EntityRenderer> customize = (_e, _r) => { };

        public SimpleAttackingLivingRenderer(string _entityName)
        {
            entityName = _entityName;
        }

        protected override void Setup(Entity _e)
        {
            for (var i = Idle; i <= Dead; i++)
            {
                var texturePostfix = i is Dead ? "dead" : i is Idle || i is Walk ? "idle" : "attack";
                var texture = Vertexer.LoadTexture(entityName + "_" + texturePostfix + ".png");
                var layout = new TextureLayout(texture, texture.width / texture.height, 1);
                var sprite = new LinearSprite(layout, layout.layout.countX, 6);

                var box = _e.AaBb;
                sprite.Scale((float) box.Width * 1.5f, (float) box.Height * 1.5f);
                sprites.Add(i, sprite);
            }

            sprites[Dead].SetNoRepeat();
            customize(_e, this);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            GL.Translate(0, _e.AaBb.Height / 2, 0);
            sprites[_e.state].Render();
            if (!(_e.state is Dead) && sprites[Dead].frozen)
                sprites[Dead].Reset();
            GL.PopMatrix();
        }
    }
}
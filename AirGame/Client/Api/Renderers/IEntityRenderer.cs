using System;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils.Math;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Renderers
{
    public abstract class EntityRenderer
    {
        public bool isSetUp;

        protected Color4 OnDamage = new Color4(1, 0, 0, 1.0f);
        protected Color4 OnHeal = new Color4(0, 1, 0, 1.0f);

        public LinearSprite spawnSprite;
        protected FontSprite Text;
        private float start = -4;

        public EntityRenderer()
        {
            Text = FontSprite.Alagard;
        }

        protected static LinearSprite SpawnSprite
        {
            get
            {
                var layout = new TextureLayout(Textures.spawn, 7, 6);
                return new LinearSprite(layout, 7 * 6, 3);
            }
        }

        public void CallSetup(Entity _e)
        {
            Setup(_e);
            isSetUp = true;

            var box = _e.AaBb;
            spawnSprite = SpawnSprite.SetNoRepeat();
            spawnSprite.Translate(new PlanarVector(6, box.Height * -128));
            spawnSprite.SetColor(new Color4(1, 1, 1, 0.8f));

            spawnSprite.Scale(8, 8);
            spawnSprite.Scale((float) box.Width, (float) box.Height);
        }

        public abstract void Setup(Entity _e);

        public void CallRender(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            Vertexer.SetColorMode(Vertexer.ColorAdditionMode.OnlyFirst);

            if (_e is EntityLiving el)
            {

                if (el.DamageTimer > 0)
                {
                    GL.PushMatrix();
                    GL.Translate(Math.Sin(Math.Abs(el.DamageTimer) * 2) * 8, (-5 + Math.Abs(el.DamageTimer)) * 16, 0);
                    el.DamageTimer -= 0.05f * Math.Sign(el.DamageTimer);
                    Text.DrawText($"{el.LastDamage}", 12, 0.85f);
                    GL.PopMatrix();
                }
                if (el.DamageTimer > 1)
                    Vertexer.Colorize(OnDamage);
                if (el.DamageTimer < -1)
                    Vertexer.Colorize(OnHeal);
            }

            if (_e.direction.Equals(Direction.Left))
                GL.Rotate(180, 0, 1, 0);
            Render(_e, _xAxis, _yAxis);
            Vertexer.BindTexture("monochromatic.png");
            //Vertexer.DrawSquare(-2, -2, 2, 2);
            //Vertexer.RenderAaBb(_e.AaBb, Chunk.BlockWidth, Chunk.BlockHeight);
            if (_e is EntityLiving)
                if (!spawnSprite.frozen)
                {
                    spawnSprite.Render();
                }

            Vertexer.ClearColor();
            Vertexer.ResetMode();
            GL.PopMatrix();
        }

        public abstract void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis);
    }
}
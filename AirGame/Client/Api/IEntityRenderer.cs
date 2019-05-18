using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API
{
    public abstract class EntityRenderer
    {
        protected const string SimpleStructPath = @"simple_structs/";
        protected const string SystemPath = @"system/";
        public bool isSetUp;

        protected Color4 OnDamage = new Color4(1, 0, 0, 1.0f);
        protected Color4 OnHeal = new Color4(0, 1, 0, 1.0f);

        public LinearSprite spawnSprite;


        protected static LinearSprite SpawnSprite
        {
            get
            {
                var layout = new TextureLayout(SystemPath + "spawn.png", 7, 6);
                return new LinearSprite(layout, 7 * 6, 10);
            }
        }

        public void CallSetup(Entity _e)
        {
            Setup(_e);
            isSetUp = true;

            spawnSprite = SpawnSprite;
            spawnSprite.MoveSpriteTo(new PlanarVector(-2, 40));
            spawnSprite.SetColor(new Color4(1, 1, 1, 0.5f));
        }

        public abstract void Setup(Entity _e);

        public void CallRender(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();

            if (_e is EntityLiving el)
            {
                if (el.DamageTimer > 0)
                    GL.Color4(OnDamage);
                if (el.DamageTimer < 0)
                    GL.Color4(OnHeal);
            }

            if (_e.direction.Equals(Direction.Left))
                GL.Rotate(180, 0, 1, 0);
            Render(_e, _xAxis, _yAxis);
            GuiUtils.RenderAaBb(_e.AaBb, Chunk.BlockWidth, Chunk.BlockHeight);
            if (_e is EntityLiving)
                if (spawnSprite.FullFrameCount < 1)
                {
                    var box = _e.AaBb;
                    GL.Scale(box.Width * 2, box.Height, 1);
                    spawnSprite.Render();
                }

            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.PopMatrix();
        }

        public abstract void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis);
    }
}
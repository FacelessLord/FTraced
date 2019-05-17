using System;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API
{
    public abstract class EntityRenderer
    {
        protected const string SimpleStructPath = @"simple_structs/";
        protected const string SystemPath = @"system/";

        protected Color4 OnDamage = new Color4(1,0,0,0.0f);
        public bool isSetUp;


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
        }

        public abstract void Setup(Entity _e);

        public void CallRender(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();


            //TODO
            if (_e is EntityLiving el && el.DamageTimer > 0)
                GL.Color3(0.5, 5, 0);


            if(_e.direction.Equals(Direction.Left))
                GL.Rotate(180, 0, 1, 0);
            Render(_e, _xAxis, _yAxis);

            GL.PopMatrix();
        }

        protected void WithDamage()
        {

        }


        public abstract void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis);
    }
}
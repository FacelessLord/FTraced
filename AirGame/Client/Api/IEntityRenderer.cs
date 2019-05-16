using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API
{
    public abstract class EntityRenderer
    {
        public bool isSetUp;

        public void CallSetup(Entity _e)
        {
            Setup(_e);
            isSetUp = true;
        }

        public abstract void Setup(Entity _e);

        public void CallRender(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            if (_e is EntityLiving el && el.IsTakingDamage)
                GL.Color3(1.0, 0.5, 0.5);
            if(_e.direction.Equals(Direction.Left))
                GL.Rotate(180, 0, 1, 0);
            Render(_e, _xAxis, _yAxis);
            GL.Color3(1, 1, 1.0);
            GL.PopMatrix();
        }

        public abstract void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis);
    }
}
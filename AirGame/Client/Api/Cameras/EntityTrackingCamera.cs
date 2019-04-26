using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Cameras
{
    public class EntityTrackingCamera : ICamera
    {
        public Entity trackedEntity;

        public EntityTrackingCamera(Entity _entity)
        {
            trackedEntity = _entity;
        }

        public void Update(GraphicWindow _window)
        {
        }

        public void PerformTranslation(GraphicWindow _window)
        {
            var width = trackedEntity.worldObj.width;
            var height = trackedEntity.worldObj.height;
            GL.Translate(-(trackedEntity.Position.x - width * 4 * 1.5) * 64,
                -(trackedEntity.Position.y - height * 4 * 1.5) * 32, 0);
            GL.Translate(-trackedEntity.velocity.x*128d, -trackedEntity.velocity.y*64d, 0);
        }
    }
}
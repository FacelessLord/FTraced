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
            GL.Translate(-(trackedEntity.Position.x) * 64,
                -(trackedEntity.Position.y) * 32, 0);
        }
    }
}
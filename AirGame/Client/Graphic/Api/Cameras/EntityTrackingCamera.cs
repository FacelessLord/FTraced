using GlLib.Common.Entities;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Cameras
{
    public class EntityTrackingCamera : ICamera
    {
        public double posX;
        public double posY;
        public Entity trackedEntity;

        public EntityTrackingCamera(Entity _entity)
        {
            trackedEntity = _entity;
            posX = _entity.Position.x;
            posY = _entity.Position.y;
        }

        public void Update(GameWindow _window)
        {
            posX = trackedEntity.Position.x;
            posY = trackedEntity.Position.y;
        }

        public void PerformTranslation(GameWindow _window)
        {
            GL.Translate(-posX * 64, -posY * 32, 0);
        }
    }
}
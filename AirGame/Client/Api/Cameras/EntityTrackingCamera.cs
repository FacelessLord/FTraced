using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Cameras
{
    public class EntityTrackingCamera : ICamera
    {
        public Entity trackedEntity;
        public double posX;
        public double posY;

        public EntityTrackingCamera(Entity _entity)
        {
            trackedEntity = _entity;
            posX = _entity.OldPosition.x;
            posY = _entity.OldPosition.y;
        }

        public void Update(GraphicWindow _window)
        {
//            posX = trackedEntity.OldPosition.x;
//            posY = trackedEntity.OldPosition.y;
        }

        public void PerformTranslation(GraphicWindow _window)
        {
            GL.Translate(-posX * 64,
                -posY * 32, 0);
        }
    }
}
using GlLib.Client.Graphic;
using GlLib.Common.Entities;

namespace GlLib.Client.Api.Cameras
{
    public class EntityTrackingCamera : ICamera
    {
        public Entity trackedEntity;
        
        public EntityTrackingCamera(Entity _entity)
        {
            
        }
        
        public void Update(GraphicWindow _window)
        {
            throw new System.NotImplementedException();
        }

        public void PerformTranslation(GraphicWindow _window)
        {
            throw new System.NotImplementedException();
        }
    }
}
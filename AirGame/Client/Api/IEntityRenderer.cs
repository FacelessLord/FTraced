using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.API
{
    public abstract class EntityRenderer
    {
        public bool isSetUp = false;

        public void CallSetup(Entity _e)
        {
            Setup(_e);
            isSetUp = true;
        }
        
        public abstract void Setup(Entity _e);

        public abstract void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis);
    }
}
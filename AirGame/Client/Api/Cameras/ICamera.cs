using GlLib.Client.Graphic;

namespace GlLib.Client.Api.Cameras
{
    public interface ICamera
    {
        void Update(GraphicWindow _window);

        void PerformTranslation(GraphicWindow _window);
    }
}
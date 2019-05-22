using OpenTK;

namespace GlLib.Client.Api.Cameras
{
    public interface ICamera
    {
        void Update(GameWindow _window);

        void PerformTranslation(GameWindow _window);
    }
}
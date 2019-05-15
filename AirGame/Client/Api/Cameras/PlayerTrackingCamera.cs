using GlLib.Common;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Cameras
{
    public class PlayerTrackingCamera : ICamera
    {
        public double posX;
        public double posY;

        public PlayerTrackingCamera()
        {
            posX = Proxy.GetClient().player.Position.x;
            posY = Proxy.GetClient().player.Position.y;
        }

        public void Update(GameWindow _window)
        {
            posX = Proxy.GetClient().player.Position.x;
            posY = Proxy.GetClient().player.Position.y;
        }

        public void PerformTranslation(GameWindow _window)
        {
            GL.Translate(-posX * 64, -posY * 32, 0);
        }
    }
}
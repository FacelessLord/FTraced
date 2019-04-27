using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Utils;
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

        public void Update(GraphicWindow _window)
        {
            posX = Proxy.GetClient().player.Position.x;
            posY = Proxy.GetClient().player.Position.y;
        }

        public void PerformTranslation(GraphicWindow _window)
        {
            SidedConsole.WriteLine(posX+"|"+posY);
            GL.Translate(-posX * 64, -posY * 32, 0);
        }
    }
}
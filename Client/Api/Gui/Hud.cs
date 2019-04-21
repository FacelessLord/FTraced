using GlLib.Client.Graphic;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.API.Gui
{
    public class Hud : Gui
    {
        public Hud()
        {
            string playerName = Proxy.GetClient().player.nickname;
            AddRectangle(16, 16+64, 11 * playerName.Length, 20, new Color(80, 80, 80, 120));
            AddText(playerName, 16+3, 16+3+64, 0, 0);
            AddRectangle(16, 16, 64, 64, new Color(80, 80, 80, 120));
            AddPicture(Vertexer.LoadTexture("head.png"), 16, 16, 64, 64);
        }
    }
}
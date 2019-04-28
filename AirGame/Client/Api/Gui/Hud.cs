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
            AddRectangle(16, 16 + 64, 11 * playerName.Length, 20, new Color(80, 80, 80, 120));
            AddText(playerName, 16 + 3, 16 + 3 + 64, 0, 0);
            AddRectangle(16, 16, 64, 64, new Color(80, 80, 80, 120));
            AddPicture(Vertexer.LoadTexture("head.png"), 16, 16, 64, 64);
            AddHorizontalBar(100, 100, 500, 50, new Color(40, 60, 240, 255));
            AddHorizontalBar(100, 150, 500, 50, new Color(240, 60, 40, 255));
            AddNumeric(350, 80, 20, 10);
        }
    }
}
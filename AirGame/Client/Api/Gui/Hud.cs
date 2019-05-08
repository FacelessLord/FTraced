using GlLib.Client.Api.Gui;
using GlLib.Client.Graphic;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.API.Gui
{
    public class Hud : GuiFrame
    {
        public Hud()
        {
            string playerName = Proxy.GetClient()?.player.nickname;
            var col = new Color(80, 80, 80, 120);
            AddRectangle(16, 16, 64, 64);
            AddRectangle(16, 20 + 64, 64, 20);
            AddText(playerName, 16, 16 + 64, 64, 30);
            AddPicture("head.png", 16, 16, 64, 64);
            AddHorizontalBar(100, 100, 500, 50, new Color(40, 60, 240, 255));
            AddHorizontalBar(100, 150, 500, 50, new Color(240, 60, 40, 255));
            AddNumeric(350, 80, 20, 10);
        }
    }
}
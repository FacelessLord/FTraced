using GlLib.Common;
using OpenTK;

namespace GlLib.Client.API.Gui
{
    public class Hud : Gui
    {
        public Hud()
        {
            string playerName = Proxy.GetClient().player.nickname;
            AddRectangle(10,10,11*playerName.Length,20, new Color(80,80,80,120));
            AddText(playerName,13,13,0,0);
        }
    }
}
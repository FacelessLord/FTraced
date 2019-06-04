using GlLib.Client.Api.Gui;
using GlLib.Common;
using GlLib.Common.SpellCastSystem;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class Hud : GuiFrame
    {
        private readonly GuiHorizontalBar cast;
        private readonly GuiHorizontalBar health;
        private readonly GuiSign moneySign;

        public Hud()
        {
            var playerName = Proxy.GetClient()?.player.nickname;
            var col = new Color(80, 80, 80, 120);
            AddRectangle(16, 16, 64, 64);
            AddRectangle(16, 20 + 64, 64, 20);
            AddText(playerName, 12, 16, 12 + 64, 64, 30);
//            AddPicture("head.png", 16, 16, 64, 64);
            cast = AddHorizontalBar(80, 48, 500, 30, new Color(240, 200, 60, 255));
            health = AddHorizontalBar(80, 16, 500, 30, new Color(240, 60, 40, 255));
            moneySign = new GuiSign(Proxy.GetWindow().Fps+ "", 12, 40, 20, 8, 8, Color.Gold);
            Add(moneySign);
            //            AddNumeric(350, 80, 20, 10);
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            health.maxValue = Proxy.GetClient().player.MaxHealth;
            health.value = Proxy.GetClient().player.Health;
            cast.maxValue = SpellSystem.MaxCastTime;
            cast.value = Proxy.GetClient().player.spells.InternalTime;
            moneySign.text = Proxy.GetWindow().Fps + "";
        }
    }
}
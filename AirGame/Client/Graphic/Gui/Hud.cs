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
        private readonly GuiSign fpsSign;
        private readonly GuiRectangle fpsRect;

        public Hud()
        {
            var playerName = Proxy.GetClient()?.player.nickname;
            var col = new Color(80, 80, 80, 120);
            var player = Proxy.GetClient().player;
            AddRectangle(16, 16, 64, 64);
            AddRectangle(84, 16, 64, 64);
            var rightHand = new GuiSlotTypeRenderer(player.equip, 0, 16 + 4 + 64, 16) {width = 64, height = 64};
            var leftHand = new GuiSlotTypeRenderer(player.equip, 1, 16, 16) {width = 64, height = 64};
            rightHand.showSelection = false;
            leftHand.showSelection = false;
            Add(rightHand);
            Add(leftHand);
            AddRectangle(16, 20 + 64, 64, 20);
            AddText(playerName, 12, 16, 12 + 64, 64, 30);
//            AddPicture("head.png", 16, 16, 64, 64);
            cast = AddHorizontalBar(80 + 68, 48, 500, 30, new Color(240, 200, 60, 255));
            health = AddHorizontalBar(80 + 68, 16, 500, 30, new Color(240, 60, 40, 255));
            fpsRect = AddRectangle(Proxy.GetWindow().Width-48, 18, 32, 16);
            fpsRect.grainSize = 8;
            fpsSign = new GuiSign(Proxy.GetWindow().Fps+ "", 12, Proxy.GetWindow().Width-40, 20, 8, 8, Color.Gold);
            Add(fpsSign);
            //            AddNumeric(350, 80, 20, 10);
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            health.maxValue = Proxy.GetClient().player.MaxHealth;
            health.value = Proxy.GetClient().player.Health;
            cast.maxValue = SpellSystem.MaxCastTime;
            cast.value = Proxy.GetClient().player.spells.InternalTime;
            fpsSign.text = Proxy.GetWindow().Fps + "";
            fpsRect.x = Proxy.GetWindow().Width - 48;
            fpsSign.x = Proxy.GetWindow().Width - 40;
        }
    }
}
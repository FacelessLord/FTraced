using GlLib.Client.Api.Gui;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class ResurrectionGui : GuiFrame
    {
        public GuiRectangle rectangle;
        public GuiButton resurrectButton;

        public ResurrectionGui()
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            SetNoClose();
            rectangle = AddRectangle(w / 4 - 10, h / 3 - 10, w / 2 + 20, h / 3);
            resurrectButton = new GuiButton("Resurrect", (w - 180) / 2, h / 3, w / 4, d);
            Add(resurrectButton);
            resurrectButton.releaseAction += (_f, _b) =>
            {
                Proxy.GetClient().ResurrectPlayer();
                Proxy.GetWindow().CloseGui(true);
            };
        }

        public override void Update(GameWindow _window)
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            rectangle.x = 3 * w / 8 - 10;
            rectangle.y = h / 3 - 10;
            rectangle.width = w / 4 + 20;
            rectangle.height = (ScreenObjects.Count - 1) * d + 20;

            resurrectButton.x = 3 * w / 8;
            resurrectButton.y = h / 3;
            resurrectButton.width = w / 4;
            resurrectButton.height = d;
        }
    }
}
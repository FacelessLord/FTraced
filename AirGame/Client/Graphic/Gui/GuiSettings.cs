using GlLib.Client.Api.Gui;
using GlLib.Client.API.Gui;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class GuiSettings : GuiFrame
    {
        public GuiRectangle rectangle;
        public GuiButton exitButton;

        public GuiSettings(GuiFrame _prev)
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            if (!(_prev.background is null))
            {
                background = _prev.background;
                Add(_prev.background);
            }
            rectangle = AddRectangle(w / 4 - 10, h / 3 - 10, w / 2 + 20, h / 3);
            exitButton = new GuiButton("Return to menu", 3 * w / 8, h / 3, w / 4, d);
            Add(exitButton);
            exitButton.releaseAction += (_f, _b) => { Proxy.GetWindow().OpenGui(_prev); };
        }

        public override void Update(GameWindow _window)
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            if (!(background is null))
            {
                background.width = w;
                background.height = h;
            }

            rectangle.x = 3 * w / 8 - 10;
            rectangle.y = h / 3 - 10;
            rectangle.width = w / 4 + 20;
            rectangle.height = d + 20; //(ScreenObjects.Count - 2) * d + 20;

            exitButton.x = 3 * w / 8;
            exitButton.y = h / 3;
            exitButton.width = w / 4;
            exitButton.height = d;
        }
    }
}
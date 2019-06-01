using System.Linq;
using GlLib.Client.Api.Gui;
using GlLib.Client.API.Gui;
using GlLib.Client.Input;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class GuiSettings : GuiFrame
    {
        public GuiButton exitButton;
        public GuiRectangle rectangle;
        public GuiPanel settings;

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
            settings = new GuiPanel(w / 4 - 10, h / 3 - 10, w / 2 + 20, h / 3);
            Add(settings);
            settings.bar = new GuiScrollBar(settings.width - 50, 0, 50, settings.height);

            var binds = KeyBinds.clickBinds.Concat(KeyBinds.binds).OrderBy(_ka => KeyBinds.delegateNames[_ka.Value])
                .ToList();
            for (int i = 0; i < binds.Count; i++)
            {
                var bind = binds[i];
                var dw = settings.width - settings.bar.width;
                var n = 3;
                var key = new GuiBindButton(bind.Key, bind.Value, 0, i * d, d * n, d);
                var sign = new GuiSign(KeyBinds.delegateNames[bind.Value], d * n, i * d, dw - d * n, d);
                settings.Add(key);
                settings.Add(sign);
            }

            settings.bar.maxValue = (int) (settings.GetPanelBox().Height - settings.GetViewbox().Height);
            exitButton = new GuiButton("Return to menu", 3 * w / 8, h / 3, w / 4, d);
            Add(exitButton);
            exitButton.releaseAction += (_f, _b) => { Proxy.GetWindow().OpenGui(_prev); };
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            var dsize = 4;

            if (!(background is null))
            {
                background.width = w;
                background.height = h;
            }

            settings.x = w / 4 - 10;
            settings.y = h / 3 - 10;
            settings.width = w / 2 + 20;
            settings.height = h / 3;

            settings.bar.x = settings.width - 50;
            settings.bar.y = 0;
            settings.bar.width = 50;
            settings.bar.height = settings.height;

            rectangle.x = settings.x - dsize / 2;
            rectangle.y = settings.y - dsize / 2;
            rectangle.width = settings.width - settings.bar.width;
            rectangle.height = settings.height + d + 20; //(ScreenObjects.Count - 2) * d + 20;

            exitButton.x = settings.x + 10 - dsize / 2;
            exitButton.y = settings.y + settings.height + 10 - dsize / 2;
            exitButton.width = settings.width - settings.bar.width - 20;
            exitButton.height = d;
        }
    }
}
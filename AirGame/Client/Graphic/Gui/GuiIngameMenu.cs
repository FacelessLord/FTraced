using GlLib.Client.Api.Gui;
using GlLib.Client.API.Gui;
using GlLib.Common;
using GlLib.Common.Map;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class GuiIngameMenu : GuiFrame
    {
        public GuiRectangle rectangle;
        public GuiButton startButton;
        public GuiButton settingsButton;
        public GuiButton saveButton;
        public GuiButton exitButton;

        public GuiIngameMenu()
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
//            background = AddPicture("background.png", 0, 0, w, h);
            rectangle = AddRectangle(w / 4 - 10, h / 3 - 10, w / 2 + 20, h / 3);
            startButton = new GuiButton("Return to Game", (w - 180) / 2, h / 3, w / 4, d);
            Add(startButton);
            startButton.releaseAction += (_f, _b) => { Proxy.GetWindow().CloseGui(); };
            saveButton = new GuiButton("Save game", (w - 180) / 2, h / 3 + 2 * d, w / 4, d);
            Add(saveButton);
            saveButton.releaseAction = (_f, _b) => { WorldManager.SaveWorld(Proxy.GetClient().world); };
            settingsButton = new GuiButton("Settings", (w - 180) / 2, h / 3 + 3 * d, w / 4, d);
            Add(settingsButton);
            settingsButton.releaseAction = (_f, _b) => { Proxy.GetWindow().OpenGui(new GuiSettings(_f)); };
            exitButton = new GuiButton("Exit", (w - 180) / 2, h / 3 + 4 * d, w / 4, d);
            Add(exitButton);
            exitButton.releaseAction = (_f, _b) =>
            {
                Core.StopWorld("Player pressed exit");
                Proxy.GetWindow().hud = null;
                Proxy.GetWindow().OpenGui(new GuiMainMenu());
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

            startButton.x = 3 * w / 8;
            startButton.y = h / 3;
            startButton.width = w / 4;
            startButton.height = d;

            saveButton.x = 3 * w / 8;
            saveButton.y = h / 3 + d;
            saveButton.width = w / 4;
            saveButton.height = d;

            settingsButton.x = 3 * w / 8;
            settingsButton.y = h / 3 + 2 * d;
            settingsButton.width = w / 4;
            settingsButton.height = d;

            exitButton.x = 3 * w / 8;
            exitButton.y = h / 3 + 3 * d;
            exitButton.width = w / 4;
            exitButton.height = d;
        }
    }
}
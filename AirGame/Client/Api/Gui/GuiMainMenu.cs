using GlLib.Client.API.Gui;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class GuiMainMenu : GuiFrame
    {
        public GuiPicture background;
        public GuiButton startButton;
        
        public GuiMainMenu()
        {
            int w = Proxy.GetWindow().Width;
            int h = Proxy.GetWindow().Height;
//            background = AddPicture("background.png", 0, 0, w, h);
            AddRectangle(w / 4 - 10, h / 3 - 10, w / 2 + 20, h / 3);
            var startButton2 = new GuiButton("Start Singleplayer Game", (w - 180) / 2, h / 4, w / 4, 40);
            Add(startButton2);
            startButton = new GuiButton("Start Singleplayer Game", (w - 180) / 2, h / 3, w / 4, 40);
            Add(startButton);
        }

        public override void Update(GameWindow _window)
        {
            int w = Proxy.GetWindow().Width;
            int h = Proxy.GetWindow().Height;
//            background.width = w;
//            background.height = h;
            startButton.x = 3*w / 8;
            startButton.y = h / 3;
            startButton.width = w / 4;
        }
    }
}
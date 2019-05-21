using GlLib.Client.Api.Gui;
using GlLib.Client.API.Gui;
using GlLib.Common;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class GuiChat : GuiFrame
    {
        public GuiChatInput chat;
        public GuiRectangle chatRect;

        public GuiChat()
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            chatRect = AddRectangle(d / 2, h - d * 2, w - d, d);
            chat = new GuiChatInput("", d * 2 / 3, h - d * 2, w - d, d);
            Add(chat);
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            chatRect.x = d / 2;
            chatRect.y = h - d * 2;
            chatRect.width = w - d;
            chatRect.height = d;
            chat.x = d * 2 / 3;
            chat.y = h - d * 2;
            chat.width = w - d;
            chat.height = d;
        }
    }
}
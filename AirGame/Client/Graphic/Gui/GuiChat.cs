using GlLib.Client.Api.Gui;
using GlLib.Common;
using GlLib.Common.Io;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class GuiChat : GuiFrame
    {
        public GuiChatInput chat;
        public GuiRectangle chatRect;
        public GuiRectangle historyRect;
        public bool justCreated = true;

        public GuiChat()
        {
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            historyRect = AddRectangle(d / 2, h - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3, w - d,
                d * (ChatIo.MaxLines + 1) * 2 / 3);
            chatRect = AddRectangle(d / 2, h - d * 2, w - d, d);
            chat = new GuiChatInput("", 12, d * 2 / 3, h - d * 2, w - d, d);
            Add(chat);
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            if (justCreated)
            {
                focusedObject = chat;
                justCreated = false;
            }

            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = h / 25;
            chatRect.x = d / 2;
            chatRect.y = h - d * 2;
            chatRect.width = w - d;
            chatRect.height = d;
            historyRect.x = d / 2;
            historyRect.y = h - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3;
            historyRect.width = w - d;
            historyRect.height = d * (ChatIo.MaxLines + 1) * 2 / 3;
            chat.x = d * 2 / 3;
            chat.y = h - d * 2;
            chat.width = w - d;
            chat.height = d;
        }
    }
}
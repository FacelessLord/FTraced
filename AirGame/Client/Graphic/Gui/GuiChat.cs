using GlLib.Client.Api.Gui;
using GlLib.Common;
using GlLib.Common.Io;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class GuiChat : GuiFrame
    {
        public GuiChatInput chat;
        public GuiChatHistory chatHistory;
        public GuiRectangle chatRect;
        public GuiRectangle historyRect;
        public bool justCreated = true;

        public GuiChat(int _width = -1, int _height = -1)
        {
            FullWidth = _width == -1;
            FullHeight = _height == -1;

            Width = _width == -1 ? Proxy.GetWindow().Width : _width;
            Height = _height == -1 ? Proxy.GetWindow().Height : _height;
            var d = Height / 25;
            historyRect = AddRectangle(d / 2, Height - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3, Width - d,
                d * (ChatIo.MaxLines + 1) * 2 / 3);
//            historyRect.grainSize = 0;
            chatRect = AddRectangle(d / 2, Height - d * 2, Width - d, d);
            chat = new GuiChatInput("", 12, d * 2, Height - d * 2, Width - d, d);
            chatHistory = new GuiChatHistory(12, d * 2 / 3, Height - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3,
                Width - d, d * (ChatIo.MaxLines + 1) * 2 / 3);
            Add(chat);
            Add(chatHistory);
        }

        public bool FullWidth { get; set; }
        public bool FullHeight { get; set; }


        public int Width { get; private set; }
        public int Height { get; private set; }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            if (justCreated)
            {
                focusedObject = chat;
                justCreated = false;
            }

            if (FullWidth) Width = Proxy.GetWindow().Width;
            if (FullHeight) Height = Proxy.GetWindow().Height;

            var d = Height / 25;

            chatRect.x = d / 2;
            chatRect.y = Height - d * 2;
            chatRect.width = Width - d;
            chatRect.height = d;

            historyRect.x = d / 2;
            historyRect.y = Height - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3;
            historyRect.width = Width - d;
            historyRect.height = d * (ChatIo.MaxLines + 1) * 2 / 3;

            chat.x = d * 2 / 3;
            chat.y = Height - d * 2;
            chat.width = Width - d;
            chat.height = d;

            chatHistory.x = d * 2 / 3;
            chatHistory.y = Height - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3;
            chatHistory.width = Width - d;
            chatHistory.height = d * (ChatIo.MaxLines + 1) * 2 / 3;
        }
    }
}
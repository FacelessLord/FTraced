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

        public int Width { get; }
        public int Height { get; }

        public GuiChat(int _width = -1, int _height = -1)
        {
            Width = _width == -1 ? Proxy.GetWindow().Width : _width;
            Height = _height == -1 ? Proxy.GetWindow().Height : _height;
            var d = Height / 25;
            historyRect = AddRectangle(d / 2, Height - d * 2 - d * (ChatIo.MaxLines + 1) * 2 / 3, Width - d,
                d * (ChatIo.MaxLines + 1) * 2 / 3, Color.FromArgb(192, 255, 255, 255));
//            historyRect.grainSize = 0;
            chatRect = AddRectangle(d / 2, Height - d * 2, Width - d, d);
            chat = new GuiChatInput("", 12, d * 2 / 3, Proxy.GetWindow().Height - d * 2, Width - d, d);
            chatHistory = new GuiChatHistory(12, d * 2 / 3, Proxy.GetWindow().Height - d * 3, Width - d, Height);
            Add(chat);
            Add(chatHistory);
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            if (justCreated)
            {
                focusedObject = chat;
                justCreated = false;
            }

            var w = Width;
            var h = Height;
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

            chat.x = d * 2 / 3;
            chat.y = Proxy.GetWindow().Height - d * 3;
            chat.width = Width - d;
            chat.height = Height;
        }
    }
}
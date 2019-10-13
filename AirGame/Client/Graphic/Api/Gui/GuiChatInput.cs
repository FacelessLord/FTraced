using System.Linq;
using GlLib.Common;
using GlLib.Common.Chat;
using GlLib.Utils.StringParser;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiChatInput : GuiText
    {
        private IParser _commandParser;
        private IParser _jsParser;
        public int historyPointer;

        public GuiChatInput(string _baseText, int _fontSize, int _x, int _y, int _width, int _height) : base(_baseText,
            _fontSize, _x, _y, _width,
            _height)
        {
            Initialize();
        }

        public GuiChatInput(string _baseText, int _fontSize, int _x, int _y, int _width, int _height, Color _color) :
            base(_baseText, _fontSize, _x, _y, _width, _height, _color)
        {
            Initialize();
        }

        private void Initialize()
        {
            _commandParser = new CommandParser();
            _jsParser = new JsParser();
        }

        public override void OnKeyDown(GuiFrame _guiFrame, KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_guiFrame, _e);

            if (_e.Key is Key.Up)
            {
                var commands = Proxy.GetClient().player.chatIo.InputStream()
                    .Where(_l => _l.StartsWith("/>") || _l.StartsWith("~>") || _l.StartsWith("!>")).ToList();
                if (historyPointer < commands.Count)
                {
                    text = commands[historyPointer++].Substring(2);
                    cursorX = text.Length;
                }
            }

            if (_e.Key is Key.Down)
            {
                var commands = Proxy.GetClient().player.chatIo.InputStream()
                    .Where(_l => _l.StartsWith("/>") || _l.StartsWith("~>") || _l.StartsWith("!>")).ToList();
                if (historyPointer > 0)
                {
                    text = commands[--historyPointer].Substring(2);
                    cursorX = text.Length;
                }
                else
                {
                    text = "";
                    cursorX = 0;
                    historyPointer = 0;
                }
            }
        }

        public override void HandleEnterKey()
        {
            base.HandleEnterKey();

            var io = Proxy.GetClient().player.chatIo;

            if (text.StartsWith('/'))
            {
                io.Output("/>" + text.Substring(1));
                _commandParser.Parse(text.Substring(1), io);
            }
            else if (text.StartsWith('~'))
            {
                io.Output("~>" + text.Substring(1));
                _jsParser.Parse(text.Substring(1), io);
            }
            else
            {
                io.Output("!>" + text);
            }

            cursorX = 0;
            historyPointer = -1;
            text = "";
        }

        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            return this;
        }
    }
}
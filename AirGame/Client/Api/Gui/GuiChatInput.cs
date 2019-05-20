using GlLib.Client.API.Gui;
using GlLib.Utils;
using GlLib.Utils.StringParser;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class GuiChatInput : GuiText
    {
        private Parser _parser = new Parser();

        public GuiChatInput(string _baseText, int _x, int _y, int _width, int _height) : base(_baseText, _x, _y, _width,
            _height)
        {
        }

        public GuiChatInput(string _baseText, int _x, int _y, int _width, int _height, Color _color) : base(_baseText,
            _x, _y, _width, _height, _color)
        {
        }

        public override void HandleEnterKey()
        {
            base.HandleEnterKey();
            _parser.AddParse("Help", _s => SidedConsole.WriteLine("This is Help"));
            SidedConsole.WriteLine("This can be Help");
            _parser.Parse(text);
            SidedConsole.WriteLine("This is not Help");
            cursorX = 0;
            text = "";
        }
    }
}
using GlLib.Client.API.Gui;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class GuiChatInput : GuiText
    {
        public GuiChatInput(string _baseText, int _x, int _y, int _width, int _height) : base(_baseText, _x, _y, _width, _height)
        {
        }

        public GuiChatInput(string _baseText, int _x, int _y, int _width, int _height, Color _color) : base(_baseText, _x, _y, _width, _height, _color)
        {
        }

        public override void HandleEnterKey()
        {
            base.HandleEnterKey();
            cursorX = 0;
            text = "";
        }
    }
}
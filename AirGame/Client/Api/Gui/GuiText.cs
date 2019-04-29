using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiSign : GuiObject
    {
        public string text;

        public GuiSign(string _text, int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            text = _text;
            font = new AlagardFontSprite();
        }

        public GuiSign(string _text, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            text = _text;
            font = new AlagardFontSprite();
        }

        public static FontSprite font;

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            var widthCenter = (width - font.GetTextWidth(text, 11)) / 2;
            var heightCenter = (height - 11d) / 2;
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);
            GL.Translate(x + widthCenter, y + heightCenter, 0);
            font.DrawText(text, 11);
            GL.Color4(1.0, 1, 1, 1);

            GL.PopMatrix();
        }
    }
}
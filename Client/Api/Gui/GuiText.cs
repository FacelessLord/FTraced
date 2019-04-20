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
        }

        public GuiSign(string _text, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            text = _text;
        }

        public static FontSprite font = new AlagardFontSprite();

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);
            GL.Translate(x, y, 0);
            font.DrawText(text, 11);
            GL.Color4(1.0, 1, 1, 1);

            GL.PopMatrix();
        }
    }
}
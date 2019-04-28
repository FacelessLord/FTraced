using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiNumeric : GuiObject
    {
        public GuiNumeric(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            font = new AlagardFontSprite();
        }

        public GuiNumeric(int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width, _height,
            _color)
        {
            font = new AlagardFontSprite();
        }

        public static FontSprite font;

        public double value = 75;
        public double maxValue = 100;

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
        }

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            string text = value + "/" + maxValue;
            GL.Translate(x - font.GetTextWidth(text, height), y, 0);
            font.DrawText(text, height, color.R, color.G, color.B, color.A);
            GL.PopMatrix();
        }
    }
}
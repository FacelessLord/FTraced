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

        public override void Update(GuiFrame _gui)
        {
            base.Update(_gui);
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            string text = value + "/" + maxValue;
            GL.Translate(x - font.GetTextWidth(text, height), y, 0);
            font.DrawText(text, height, color.R, color.G, color.B, color.A);
            GL.PopMatrix();
        }
    }
}
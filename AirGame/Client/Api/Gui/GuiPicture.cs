using GlLib.Client.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Gui
{
    public class GuiPicture : GuiObject
    {
        public Texture texture;

        public GuiPicture(Texture _texture, int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            texture = _texture;
        }

        public GuiPicture(Texture _texture, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            texture = _texture;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            Vertexer.Colorize(color);
            Vertexer.BindTexture(texture);
            Vertexer.DrawSquare(x, y, x + width, y + height);
            Vertexer.ClearColor();

            GL.PopMatrix();
        }
    }
}
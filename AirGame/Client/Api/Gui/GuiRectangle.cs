using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiRectangle : GuiObject
    {
        public GuiRectangle(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            var texture = Vertexer.LoadTexture("gui/window_back.png");
            background = new TextureLayout(texture, 0, 0, 96, 96, 3, 3);
        }

        public GuiRectangle(int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            var texture = Vertexer.LoadTexture("gui/window_back.png");
            background = new TextureLayout(texture, 0, 0, 96, 96, 3, 3);
        }

        public TextureLayout background;

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);

            GuiUtils.DrawSizedSquare(background, x, y, width, height, 0.0625f);
            
            GL.Color4(1.0, 1, 1, 1);

            GL.PopMatrix();
        }
    }
}
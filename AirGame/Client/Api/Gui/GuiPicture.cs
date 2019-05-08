using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiPicture : GuiObject
    {
        public GuiPicture(string _texture, int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            texture = Vertexer.LoadTexture(_texture);
        }

        public GuiPicture(string _texture, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            texture = Vertexer.LoadTexture(_texture);
        }

        public Texture texture;

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);

            Vertexer.BindTexture(texture);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(x, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x, y + height, 0, 1);

            Vertexer.Draw();
            GL.Color4(1.0, 1, 1, 1);

            GL.PopMatrix();
        }
    }
}
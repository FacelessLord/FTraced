using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class PictureSprite : ISprite
    {
        public int height;
        public Texture texture;
        public int width;

        public PictureSprite(string _textureName, int _width = GuiSlot.SlotSize, int _height
            = GuiSlot.SlotSize)
        {
            texture = Vertexer.LoadTexture(_textureName);
            width = _width;
            height = _height;
        }

        public void Render()
        {
            GL.PushMatrix();
            Vertexer.BindTexture(texture);
            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(0, 0, 0, 0);
            Vertexer.VertexWithUvAt(width, 0, 1, 0);
            Vertexer.VertexWithUvAt(width, height, 1, 1);
            Vertexer.VertexWithUvAt(0, height, 0, 1);

            Vertexer.Draw();
            GL.PopMatrix();
        }
    }
}
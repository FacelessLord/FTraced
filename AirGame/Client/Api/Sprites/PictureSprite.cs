using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class PictureSprite : Sprite
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
            scale = new Vector3(1, 1, 1);
        }

        protected override void RenderSprite()
        {
            Vertexer.BindTexture(texture);
            Vertexer.DrawSquare(-width / 2d, -height / 2d, width / 2d, height / 2d);
        }
    }
}
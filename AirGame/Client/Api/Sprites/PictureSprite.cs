using GlLib.Client.Api.Gui;
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

        public PictureSprite(Texture _texture, int _width = GuiSlot.SlotSize, int _height
            = GuiSlot.SlotSize)
        {
            texture = _texture;
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
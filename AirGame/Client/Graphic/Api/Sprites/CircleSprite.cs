using GlLib.Client.Graphic;

namespace GlLib.Client.Api.Sprites
{
    public class CircleSprite : Sprite
    {
        public int accuracy;

        public CircleSprite()
        {
            accuracy = 16;
        }

        public CircleSprite(int _accuracy)
        {
            accuracy = _accuracy;
        }

        protected override void RenderSprite()
        {
            Vertexer.BindTexture("monochromatic.png");
            Vertexer.DrawCircle(accuracy);
        }
    }
}
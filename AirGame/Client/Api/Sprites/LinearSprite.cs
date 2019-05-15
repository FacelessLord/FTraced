using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class LinearSprite : ISprite
    {
        public int frameCount;
        public int maxFrameCount;
        public int step;
        public TextureLayout texture;
        public bool frozen = false;

        public LinearSprite(TextureLayout _texture, int _maxFrameCount, int _step = 1, int _frameCount = 0)
        {
            texture = _texture;
            frameCount = _frameCount;
            maxFrameCount = _maxFrameCount;
            step = _step;
        }

        public void Render()
        {
            GL.PushMatrix();
            GL.Translate(-texture.layout.FrameWidth(), -texture.layout.FrameHeight(), 0);
            texture.Render(frameCount / step);
            if (!frozen)
                frameCount = (frameCount + 1) % (maxFrameCount * step);
            GL.PopMatrix();
        }

        public LinearSprite SetFrozen(bool _freeze = true)
        {
            frozen = _freeze;
            return this;
        }
    }
}
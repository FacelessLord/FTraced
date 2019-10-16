namespace GlLib.Client.Api.Sprites
{
    public class LinearSprite : Sprite
    {
        private long frameCount;
        public bool frozen;

        public int maxFrameCount;
        public bool noRepeat;

        public int step;
        public TextureLayout texture;

        public LinearSprite(TextureLayout _texture, int _maxFrameCount, int _step = 1, int _frameCount = 0)
        {
            frameCount = 0;
            texture = _texture;
            maxFrameCount = _maxFrameCount;
            step = _step;
        }

        public long FullFrameCount => frameCount / maxFrameCount / step;

        protected override void RenderSprite()
        {
            texture.Render(frameCount / step);
            if (!frozen)
                if (noRepeat && frameCount + 1 >= maxFrameCount * step)
                    SetFrozen();
                else
                    frameCount = (frameCount + 1) % (maxFrameCount * step);
        }

        public LinearSprite SetFrozen(bool _freeze = true)
        {
            frozen = _freeze;
            return this;
        }

        public LinearSprite SetNoRepeat(bool _no = true)
        {
            noRepeat = _no;
            return this;
        }

        public void Reset()
        {
            frameCount = 0;
            frozen = false;
        }
    }
}
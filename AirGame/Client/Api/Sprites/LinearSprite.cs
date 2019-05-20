using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class LinearSprite : ISprite
    {
        private long _frameCount;
        private PlanarVector _moveTo;
        private Color4 color = new Color4(1, 1, 1, 1.0f);
        public int frameCount;
        public bool frozen;

        private bool hasColor;
        public int maxFrameCount;
        public bool noRepeat;

        private Vector3 scale;
        public int step;
        public TextureLayout texture;

        // ReSharper disable once InconsistentNaming
        private float transparency;

        public LinearSprite(TextureLayout _texture, int _maxFrameCount, int _step = 1, int _frameCount = 0)
        {
            transparency = 1;
            _frameCount = 0;
            texture = _texture;
            frameCount = _frameCount;
            maxFrameCount = _maxFrameCount;
            step = _step;

            _moveTo = new PlanarVector(0, 0);
            scale = new Vector3(1, 1, 0);
        }

        public long FullFrameCount => _frameCount / maxFrameCount / step;

        public void Render()
        {
            _frameCount++;
            GL.PushMatrix();
            GL.Translate(-texture.layout.FrameWidth() - _moveTo.x,
                -texture.layout.FrameHeight() - _moveTo.y,0);

            GL.Scale(scale);
            Vertexer.Colorize(color);
            texture.Render(frameCount / step);
            if (!frozen)
                if (noRepeat && frameCount + 1 >= maxFrameCount * step)
                    SetFrozen();
                else
                    frameCount = (frameCount + 1) % (maxFrameCount * step);
            Vertexer.ClearColor();
            GL.PopMatrix();
        }

        public void MoveSpriteTo(PlanarVector _vector)
        {
            _moveTo = _vector;
        }

        public void SetSize(Vector3 _scale)
        {
            scale = _scale;
        }

        public void SetColor(Color4 _color)
        {
            hasColor = true;
            color = _color;
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
            _frameCount = 0;
            frozen = false;
        }
    }
}
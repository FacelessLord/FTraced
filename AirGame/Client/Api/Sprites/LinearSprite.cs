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


        public LinearSprite(TextureLayout _texture, int _maxFrameCount, int _step = 1, int _frameCount = 0)
        {
            _frameCount = 0;
            texture = _texture;
            frameCount = _frameCount;
            maxFrameCount = _maxFrameCount;
            step = _step;

            _moveTo = new PlanarVector(0, 0);
            scale = new Vector3(32, 32, 1);
        }

        public long FullFrameCount => _frameCount / maxFrameCount / step;

        public void Render()
        {
            _frameCount++;
            GL.PushMatrix();
            GL.Translate(_moveTo.x, _moveTo.y, 0);

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

        public LinearSprite Translate(float _x, float _y)
        {
            _moveTo += new PlanarVector(_x,_y);
            return this;
        }

        public LinearSprite Translate(PlanarVector _vector)
        {
            _moveTo += _vector;
            return this;
        }

        public LinearSprite Scale(float _x, float _y)
        {
            scale = new Vector3(_x * scale.X, _y * scale.Y, 1);
            return this;
        }

        public LinearSprite Scale(Vector3 _scale)
        {
            scale = _scale;
            return this;
        }

        public LinearSprite SetColor(Color4 _color)
        {
            hasColor = true;
            color = _color;
            return this;
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
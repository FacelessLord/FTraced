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
                -texture.layout.FrameHeight() - _moveTo.y,
                0);

//            GL.Enable(EnableCap.Blend);
//            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Scale(scale);
            if (hasColor)
                GL.Color4(color);
            texture.Render(frameCount / step);
            if (!frozen)
                frameCount = (frameCount + 1) % (maxFrameCount * step);
            //GL.ClearColor(1, 1, 1, transparency);
//            GL.Disable(EnableCap.Blend);
            GL.Color4(1.0f, 1, 1, 1);
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
    }
}
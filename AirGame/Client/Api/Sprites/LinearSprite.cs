using System;
using GlLib.Utils;
using OpenTK.Graphics;
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
        private PlanarVector _moveTo ;
        private Color4 color;
        // ReSharper disable once InconsistentNaming
        private float transparency;

        private long _frameCount;
        public long FullFrameCount
        {
            get => _frameCount / maxFrameCount / step;
        }

        public LinearSprite(TextureLayout _texture, int _maxFrameCount, int _step = 1, int _frameCount = 0)
        {
            transparency = 1;
            color = new Color4(1,1,1,1.0f);
            _frameCount = 0;
            texture = _texture;
            frameCount = _frameCount;
            maxFrameCount = _maxFrameCount;
            step = _step;
            _moveTo = new PlanarVector(0,0);
        }

        public void MoveSpriteTo(PlanarVector _vector)
        {
            _moveTo = _vector;
        }


#if false

        public void SetTransparency(float _transparency)
        {
            transparency = _transparency;
        }
#endif

        public void SetColor(Color4 _color)
        {

            color = _color;
        }

        public void Render()
        {
            _frameCount++;
            GL.PushMatrix();
            GL.Translate(-texture.layout.FrameWidth() - _moveTo.x,
                -texture.layout.FrameHeight() - _moveTo.y,
                0);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Color4(color);
            texture.Render(frameCount / step);
            if (!frozen)
                frameCount = (frameCount + 1) % (maxFrameCount * step);
            //GL.ClearColor(1, 1, 1, transparency);
            GL.Disable(EnableCap.Blend);
           // GL.ClearColor(255, 255, 255, 255);
            GL.PopMatrix();
        }

        public LinearSprite SetFrozen(bool _freeze = true)
        {
            frozen = _freeze;
            return this;
        }
    }
}
using GlLib.Client.Graphic;
using GlLib.Utils.Math;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public abstract class Sprite
    {
        private const float Depth = 0.866025f;
        protected Color4 color = new Color4(1, 1, 1, 1.0f);
        protected PlanarVector moveTo;

        protected Vector3 scale;
        // Move it out to constants file
        // This is the sin(2PI/3)

        public Sprite()
        {
            moveTo = new PlanarVector(0);
            scale = new Vector3(32, 32, 1);
        }

        public void Render()
        {
            GL.PushMatrix();
            GL.Translate(moveTo.x, moveTo.y, -moveTo.x * Depth);

            GL.Scale(scale);
            Vertexer.Colorize(color);
            RenderSprite();

            Vertexer.ClearColor();
            GL.PopMatrix();
        }

        protected abstract void RenderSprite();

        public Sprite Translate(float _x, float _y)
        {
            moveTo += new PlanarVector(_x, _y);
            return this;
        }

        public Sprite Translate(PlanarVector _vector)
        {
            moveTo += _vector;
            return this;
        }

        public Sprite Scale(float _x, float _y)
        {
            scale = new Vector3(_x * scale.X, _y * scale.Y, 1);
            return this;
        }

        public Sprite Scale(Vector3 _scale)
        {
            scale = _scale;
            return this;
        }

        public Sprite SetColor(Color4 _color)
        {
            color = _color;
            return this;
        }
    }
}
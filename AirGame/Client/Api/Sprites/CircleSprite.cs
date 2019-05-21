using System;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class CircleSprite : ISprite
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

        public void Render()
        {
            Vertexer.BindTexture("monochromatic.png");
            Vertexer.DrawCircle(accuracy);
        }
    }
}
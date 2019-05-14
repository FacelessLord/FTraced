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
            GL.PushMatrix();
            var t = Vertexer.LoadTexture("monochromatic.png");
            Vertexer.BindTexture(t);
            Vertexer.StartDrawing(PrimitiveType.TriangleFan);

            var angleStep = 2 * Math.PI / accuracy;
            var r = 1;
            for (var i = accuracy - 1; i >= 0; i--)
                Vertexer.VertexAt(r * Math.Cos(angleStep * i), r * Math.Sin(angleStep * i));

            Vertexer.Draw();

            GL.PopMatrix();
        }
    }
}
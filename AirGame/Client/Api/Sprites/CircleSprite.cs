using System;
using GlLib.Client.API;
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
            Texture t = Vertexer.LoadTexture("monochromatic.png");
            Vertexer.BindTexture(t);
            Vertexer.StartDrawing(PrimitiveType.TriangleFan);

            double angleStep = 2 * Math.PI / accuracy;
            int r = 1;
            for (int i = accuracy-1; i >=0; i--)
            {
                Vertexer.VertexAt(r*Math.Cos(angleStep*i),r*Math.Sin(angleStep*i));
            }
            
            Vertexer.Draw();
            
            GL.PopMatrix();
        }
    }
}
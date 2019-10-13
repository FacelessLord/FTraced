using GlLib.Client.Api.Renderers;
using GlLib.Common.Entities;
using GlLib.Utils.Math;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class StandardRenderer : EntityRenderer
    {
        private Texture _texture;

        protected override void Setup(Entity _e)
        {
            _texture = Textures.monochromatic;
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            Vertexer.BindTexture(_texture);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(10, -10, 1, 0);
            Vertexer.VertexWithUvAt(10, 10, 1, 1);
            Vertexer.VertexWithUvAt(-10, 10, 0, 1);
            Vertexer.VertexWithUvAt(-10, -10, 0, 0);

            Vertexer.Draw();
            GL.PopMatrix();
        }
    }
}
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class WorldRenderer
    {
        private readonly World _world;

        public WorldRenderer(World _world)
        {
            this._world = _world;
        }

        public void Render(double _x, double _y)
        {
            lock (_world.chunks)
            {
                var width = _world.width;
                var height = _world.height;
                var xAxis = new PlanarVector(Chunk.BlockWidth, 0);
                var yAxis = new PlanarVector(0, Chunk.BlockHeight);

                for (var i = 0; i < width; i++)
                for (var j = height - 1; j >= 0; j--)
                    if (_world[i, j].isLoaded)
                        _world[i, j].RenderChunk(i, j, xAxis, yAxis);

                //rendering entities
                GL.PushMatrix();
                for (var i = 0; i < width; i++)
                for (var j = height - 1; j >= 0; j--)
                {
                    var chunk = _world[i, j];
                    if (chunk.isLoaded)
                        foreach (var entity in chunk.entities)
                        {
                            if (!entity.GetRenderer().isSetUp)
                                entity.GetRenderer().CallSetup(entity);

                            var coord = xAxis * entity.Position.x + yAxis * entity.Position.y;
                            GL.PushMatrix();

                            GL.Translate(coord.x, coord.y, 0);
                            GL.Scale(1.5, 1.5, 1);
                            entity.GetRenderer().CallRender(entity, xAxis, yAxis);
                            GL.PopMatrix();
                        }
                }
            }

            GL.PopMatrix();
        }
    }
}
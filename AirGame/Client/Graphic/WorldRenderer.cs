using System;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class WorldRenderer
    {
        private World world;

        public WorldRenderer(World _world)
        {
            world = _world;
        }

        public void Render(double _x, double _y)
        {

            lock (world.chunks)
            {
                int width = world.width;
                int height = world.height;
                var xAxis = new PlanarVector(Chunk.BlockWidth, 0);
                var yAxis = new PlanarVector(0, Chunk.BlockHeight);

                for (var i = 0; i < width; i++)
                for (var j = height - 1; j >= 0; j--)
                    if (world[i, j].isLoaded)
                        world[i, j].RenderChunk(i, j, xAxis, yAxis);

                //rendering entities
                GL.PushMatrix();
                for (var i = 0; i < width; i++)
                for (var j = height - 1; j >= 0; j--)
                {
                    var chunk = world[i, j];
                    if (chunk.isLoaded)
                        foreach (var level in chunk.entities)
                        foreach (var entity in level)
                        {
                            var coord = xAxis * (entity.Position.x) + yAxis * (entity.Position.y);
                            GL.PushMatrix();

                            GL.Translate(coord.x, coord.y, 0);
                            GL.Scale(1.5, 1.5, 1);
                            entity.Render(xAxis, yAxis);
                            GL.PopMatrix();
                        }
                }
            }

            GL.PopMatrix();
        }
    }
}
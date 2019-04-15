using System;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class WorldRenderer
    {
        private World _world;

        public WorldRenderer(World _world)
        {
            this._world = _world;
        }

        public void Render(int _x, int _y)
        {
            int width = _world.width;
            int height = _world.height;
            var xAxis = new PlanarVector(Chunk.BlockWidth / 2, Chunk.BlockHeight / 2);
            var yAxis = new PlanarVector(Chunk.BlockWidth / 2, -Chunk.BlockHeight / 2);

            GL.PushMatrix();
            GL.Translate(-Math.Max(width, height) * Chunk.BlockWidth * 5, 0, 0);
            for (var i = 0; i < width; i++)
            for (var j = width - 1; j >= 0; j--)
                if (_world[i + _x, j + _y].isLoaded)
                    _world[i + _x, j + _y].RenderChunk(i, j, xAxis, yAxis);

            //rendering entities
            for (var i = 0; i < width; i++)
            for (var j = width - 1; j >= 0; j--)
                if (_world[i + _x, j + _y].isLoaded)
                    foreach (var level in _world[i + _x, j + _y].entities)
                    foreach (var entity in level)
                    {
                        var coord = xAxis * (entity.Position.x - 8) + yAxis * (entity.Position.y - 8);
                        GL.PushMatrix();

                        GL.Translate(coord.x, coord.y, 0);
                        GL.Scale(1.5,1.5,1);
                        entity.Render(xAxis, yAxis);
                        GL.PopMatrix();
                    }

            GL.PopMatrix();
        }
    }
}
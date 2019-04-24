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

        public void Render(double _x, double _y)
        {
            int width = _world.width;
            int height = _world.height;
            var xAxis = new PlanarVector(Chunk.BlockWidth,0 );
            var yAxis = new PlanarVector(0, Chunk.BlockHeight);

            GL.PushMatrix();
            GL.Translate(_x,_y, 0);
            for (var i = 0; i < width; i++)
            for (var j = height - 1; j >= 0; j--)
                if (_world[i, j].isLoaded)
                    _world[i, j].RenderChunk(i, j, xAxis, yAxis);

            //rendering entities
            for (var i = 0; i < width; i++)
            for (var j = height - 1; j >= 0; j--)
                if (_world[i, j].isLoaded)
                    foreach (var level in _world[i, j].entities)
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
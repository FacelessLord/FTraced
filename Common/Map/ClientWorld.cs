using System;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Map
{
    public class ClientWorld : World
    {
        public ClientWorld(string mapName, int worldId) : base(mapName, worldId)
        {
        }


        //To ClientWorld
        public void Render(int x, int y)
        {
            var xAxis = new PlanarVector(Chunk.BlockWidth / 2, Chunk.BlockHeight / 2);
            var yAxis = new PlanarVector(Chunk.BlockWidth / 2, -Chunk.BlockHeight / 2);
            GL.PushMatrix();
            GL.Translate(-Math.Max(width, height) * Chunk.BlockWidth * 5, 0, 0);
            for (var i = 0; i < width; i++)
            for (var j = width - 1; j >= 0; j--)
                if (this[i + x, j + y].isLoaded)
                    this[i + x, j + y].RenderChunk(i, j, xAxis, yAxis);

            for (var i = 0; i < width; i++)
            for (var j = width - 1; j >= 0; j--)
                if (this[i + x, j + y].isLoaded)
                    foreach (var level in this[i + x, j + y].entities)
                    foreach (var entity in level)
                    {
                        var coord = xAxis * (entity.Position.x - 8) + yAxis * (entity.Position.y - 8);
                        GL.PushMatrix();

                        GL.Translate(coord.x, coord.y, 0);
                        entity.Render(xAxis, yAxis);
                        GL.PopMatrix();
                    }

            GL.PopMatrix();
        }
    }
}
using System;
using GlLib.Graphic;

namespace GlLib.Map
{
    public class Path : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.path";
        }

        public override Texture GetTexture(int x, int y)
        {
            return Vertexer.LoadTexture("path.png");
        }
    }
}
using GlLib.Graphic;

namespace GlLib.Map
{
    public class GrassBlock:TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass";
        }

        public override Texture GetTexture(int x, int y)
        {
            return Vertexer.LoadTexture("grass.png");
        }
    }
}
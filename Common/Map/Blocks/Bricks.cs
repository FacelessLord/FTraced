using GlLib.Client.Graphic;

namespace GlLib.Common.Map.Blocks
{
    public class Bricks : TerrainBlock
    {
        public override string GetName()
        {
            return "block.indoor.bricks";
        }

        public override Texture GetTexture(World world, int x, int y)
        {
            return Vertexer.LoadTexture("bricks_worn.png");
        }
    }
}
namespace GlLib.Common.Map.Blocks
{
    public class Bricks : TerrainBlock
    {
        public override string GetName()
        {
            return "block.indoor.bricks";
        }

        public override string GetTextureName(World world, int x, int y)
        {
            return "bricks_worn.png";
        }
    }
}
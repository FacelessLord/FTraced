namespace GlLib.Common.Map.Blocks
{
    public class Bricks : TerrainBlock
    {
        public override string GetName()
        {
            return "block.indoor.bricks";
        }

        public override string GetTextureName(World _world, int _x, int _y)
        {
            return "bricks_worn.png";
        }
    }
}
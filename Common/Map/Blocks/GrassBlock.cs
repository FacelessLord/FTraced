namespace GlLib.Common.Map.Blocks
{
    public class GrassBlock : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass";
        }

        public override string GetTextureName(World _world, int _x, int _y)
        {
            return "grass.png";
        }
    }
}
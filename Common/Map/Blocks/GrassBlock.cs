namespace GlLib.Common.Map.Blocks
{
    public class GrassBlock : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass";
        }

        public override string GetTextureName(ClientWorld world, int x, int y)
        {
            return "grass.png";
        }
    }
}
namespace GlLib.Common.Map.Blocks
{
    public class AutumnGrassBlock : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.autumn";
        }

        public override string GetTextureName(ClientWorld world, int x, int y)
        {
            return "grass_autumn.png";
        }
    }
}
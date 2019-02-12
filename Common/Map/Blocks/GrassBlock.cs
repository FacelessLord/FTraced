using GlLib.Client.Graphic;

namespace GlLib.Common.Map.Blocks
{
    public class GrassBlock:TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass";
        }

        public override string GetTextureName(World world,int x, int y)
        {
            return "grass.png";
        }
    }
}
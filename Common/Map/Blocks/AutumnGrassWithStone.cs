using System;

namespace GlLib.Common.Map.Blocks
{
    public class AutumnGrassWithStone : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.autumn.stone";
        }

        public override string GetTextureName(World world, int x, int y)
        {
            return "grass_autumn_st_" + Math.Abs(Math.Round(Math.Cos(x) + Math.Sin(y)) % 2) + ".png";
        }
    }
}
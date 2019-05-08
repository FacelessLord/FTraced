using System;

namespace GlLib.Common.Map.Blocks
{
    public class AutumnGrassWithStone : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.autumn.stone";
        }

        public override string GetTextureName(World _world, int _x, int _y)
        {
            return "grass_autumn_st_" + Math.Abs(Math.Round(Math.Cos(_x) + Math.Sin(_y)) % 2) + ".png";
        }
    }
}
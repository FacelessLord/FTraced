using GlLib.Common.Map;

namespace GlLib.Common.Blocks
{
    internal class DarkGrassCoastRight : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.outdoor.darkgrasscoastright";

        public override string TextureName { get; internal set; } = Path + "DarkGrassCoastRight.png";
    }
}
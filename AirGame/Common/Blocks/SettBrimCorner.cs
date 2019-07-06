using GlLib.Common.Map;

namespace GlLib.Common.Blocks
{
    public class SettBrimCorner : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.indoor.settbrimcorner";

        public override string TextureName { get; internal set; } = Path + "SettBrimCorner.png";
    }
}
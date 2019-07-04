using GlLib.Common.Map;

namespace GlLib.Common.Blocks
{
    public class SettBrimDown : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.indoor.settbrimdown";

        public override string TextureName { get; internal set; } = Path + "SettBrimDown.png";
    }
}
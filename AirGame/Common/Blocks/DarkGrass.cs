using GlLib.Common.Map;

namespace GlLib.Common.Blocks
{
    public class DarkGrass : TerrainBlock
    {

        public override string Name { get; protected set; } = "block.outdoor.darkgrass";

        public override string TextureName { get; internal set; } = Path + "DarkGrass.png";
    }
}
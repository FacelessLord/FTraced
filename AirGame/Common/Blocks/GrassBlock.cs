namespace GlLib.Common.Map.Blocks
{
    public class GrassBlock : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.outdoor.grass";

        public override string TextureName { get; internal set; } = "grass.png";
    }
}
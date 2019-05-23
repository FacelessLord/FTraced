namespace GlLib.Common.Map.Blocks
{
    public class Path : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.outdoor.grass.path";

        public override string TextureName { get; internal set; } = "path.png";
    }
}
namespace GlLib.Common.Map.Blocks
{
    public class Bricks : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.indoor.bricks";

        public override string TextureName { get; internal set; } = "bricks_worn.png";
    }
}
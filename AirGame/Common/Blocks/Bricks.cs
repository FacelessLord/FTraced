namespace GlLib.Common.Map.Blocks
{
    public class Bricks : TerrainBlock
    {
        public Bricks()
        {
            Rotation = 90;
        }

        public override string Name { get; protected set; } = "block.indoor.bricks";

        public override string TextureName { get; internal set; } = "bricks_worn.png";
    }
}
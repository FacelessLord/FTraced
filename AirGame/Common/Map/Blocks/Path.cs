namespace GlLib.Common.Map.Blocks
{
    public class Path : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.path";
        }

        public override string GetTextureName(World _world, int _x, int _y)
        {
            return "path.png";
        }
    }
}
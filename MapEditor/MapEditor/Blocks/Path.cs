namespace MapEditor
{
    public class Path : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.path";
        }

        public override string GetTextureName(int _x, int _y)
        {
            return "path.png";
        }
    }
}
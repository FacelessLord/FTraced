namespace MapEditor
{
    public class GrassBlock : TerrainBlock
    {
        public override string GetName()
        {
            return "grass";
        }

        public override string GetTextureName( int _x, int _y)
        {
            return "grass.png";
        }
    }
}
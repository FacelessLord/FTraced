namespace MapEditor
{
    public class Bricks : TerrainBlock
    {
        public override string GetName()
        {
            return "bricks";
        }

        public override string GetTextureName( int _x, int _y)
        {
            return "bricks_worn.png";
        }
    }
}
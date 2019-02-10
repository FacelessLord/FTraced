using GlLib.Graphic;

namespace GlLib.Map
{
    public class AutumnGrassBlock:TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.autumn";
        }

        public override Texture GetTexture(World world,int x, int y)
        {
            return Vertexer.LoadTexture("grass_autumn.png");
        }
    }
}
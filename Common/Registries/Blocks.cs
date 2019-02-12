using GlLib.Common.Map;
using GlLib.Common.Map.Blocks;

namespace GlLib.Common.Registries
{
    public class Blocks
    {
        
        public static readonly TerrainBlock Grass = new GrassBlock();
        public static readonly TerrainBlock AutumnGrass = new AutumnGrassBlock();
        public static readonly TerrainBlock AutumnGrassStone = new AutumnGrassWithStone();
        public static readonly TerrainBlock Path = new Path();
        public static readonly TerrainBlock Bricks = new Bricks();

        public static void Register()
        {
            GameRegistry.RegisterBlock(Grass);
            GameRegistry.RegisterBlock(AutumnGrass);
            GameRegistry.RegisterBlock(AutumnGrassStone);
            GameRegistry.RegisterBlock(Path);
            GameRegistry.RegisterBlock(Bricks);
        }
    }
}
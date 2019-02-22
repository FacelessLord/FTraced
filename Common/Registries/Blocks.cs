using GlLib.Common.Map;
using GlLib.Common.Map.Blocks;

namespace GlLib.Common.Registries
{
    public class Blocks
    {
        public Blocks(GameRegistry registry)
        {
            _registry = registry;
        }
        
        public static readonly TerrainBlock Grass = new GrassBlock();
        public static readonly TerrainBlock AutumnGrass = new AutumnGrassBlock();
        public static readonly TerrainBlock AutumnGrassStone = new AutumnGrassWithStone();
        public static readonly TerrainBlock Path = new Path();
        public static readonly TerrainBlock Bricks = new Bricks();

        public GameRegistry _registry;
        
        public void Register()
        {
            _registry.RegisterBlock(Grass);
            _registry.RegisterBlock(AutumnGrass);
            _registry.RegisterBlock(AutumnGrassStone);
            _registry.RegisterBlock(Path);
            _registry.RegisterBlock(Bricks);
        }
    }
}
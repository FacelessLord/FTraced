using GlLib.Common.Map;
using GlLib.Common.Map.Blocks;

namespace GlLib.Common.Registries
{
    public class BlocksRegistry
    {
        public static readonly TerrainBlock Grass = new GrassBlock();
        public static readonly TerrainBlock AutumnGrass = new AutumnGrassBlock();
        public static readonly TerrainBlock AutumnGrassStone = new AutumnGrassWithStone();
        public static readonly TerrainBlock Path = new Path();
        public static readonly TerrainBlock Bricks = new Bricks();

        public GameRegistry registry;

        public BlocksRegistry(GameRegistry _registry)
        {
            this.registry = _registry;
        }

        public void Register()
        {
            registry.RegisterBlock(Grass);
            registry.RegisterBlock(AutumnGrass);
            registry.RegisterBlock(AutumnGrassStone);
            registry.RegisterBlock(Path);
            registry.RegisterBlock(Bricks);
        }
    }
}
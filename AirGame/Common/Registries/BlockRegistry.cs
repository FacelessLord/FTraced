using GlLib.Common.Blocks;
using GlLib.Common.Map;

namespace GlLib.Common.Registries
{
    public class BlocksRegistry
    {
        public static readonly TerrainBlock Grass = new GrassBlock();
        public static readonly TerrainBlock Path = new Path();
        public static readonly TerrainBlock Bricks = new Bricks();
        public static readonly TerrainBlock DarkGrass = new DarkGrass();
        public static readonly TerrainBlock DarkGrassCoastRight = new DarkGrassCoastRight();
        public static readonly TerrainBlock SettBrimCorner = new SettBrimCorner();
        public static readonly TerrainBlock SettBrimDown = new SettBrimDown();
        public static readonly TerrainBlock VoidBlock = new VoidBlock();
        public static readonly TerrainBlock CastleBlock = new CastleBlock();

        public GameRegistry registry;

        public BlocksRegistry(GameRegistry _registry)
        {
            registry = _registry;
        }

        public void Register()
        {
            registry.RegisterBlock(VoidBlock);
            registry.RegisterBlock(Grass);
            registry.RegisterBlock(Path);
            registry.RegisterBlock(Bricks);
            registry.RegisterBlock(DarkGrass);
            registry.RegisterBlock(DarkGrassCoastRight);
            registry.RegisterBlock(SettBrimCorner);
            registry.RegisterBlock(SettBrimDown);
            registry.RegisterBlock(CastleBlock);

            Stash.UpdateStash(registry);
        }
    }
}
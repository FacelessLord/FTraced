using GlLib.Common.Map;
using GlLib.Common.Map.Blocks;

namespace GlLib.Common.Registries
{
    public class Blocks
    {
        public static readonly TerrainBlock Grass = new GrassBlock();
        public static readonly TerrainBlock Path = new Path();
        public static readonly TerrainBlock Bricks = new Bricks();

        public GameRegistry registry;

        public Blocks(GameRegistry _registry)
        {
            this.registry = _registry;
        }

        public void Register()
        {
            registry.RegisterBlock(Grass);
            registry.RegisterBlock(Path);
            registry.RegisterBlock(Bricks);
            Stash.UpdateStash();
        }
    }
}
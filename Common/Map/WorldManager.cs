using System.Net.Json;

namespace GlLib.Common.Map
{
    public static class WorldManager
    {
        public static void LoadWorld(World world, JsonObjectCollection worldCollection)
        {
            world.jsonObj = worldCollection;
            world.width = (int) ((JsonNumericValue) worldCollection[0]).Value;
            world.height = (int) ((JsonNumericValue) worldCollection[1]).Value;

            world.chunks = new Chunk[world.width, world.height];

            for (var i = 0; i < world.width; i++)
            for (var j = 0; j < world.height; j++)
                world[i, j] = new Chunk(world, i, j);
            world.LoadWorld();
        }

        public static void LoadEntities(ServerWorld world, JsonObjectCollection entityCollection)
        {
        }
    }
}
namespace GlLib.Common.Map
{
    public class WorldInfo
    {
        public int width;
        public int height;

        public int maxEntityCount = 200;
        public int entityCount;

        public string mapName;

        public int worldId;

        public WorldInfo(World w)
        {
            width = w.width;
            height = w.height;
            maxEntityCount = w.MaxEntityCount;
            entityCount = w.EntityCount;
            mapName = w.mapName;
            worldId = w.worldId;
        }

        public WorldInfo()
        {
            width = 1;
            height = 1;
            maxEntityCount = 200;
            entityCount = 0;
            mapName = "null_world";
            worldId = 0;
        }
    }
}
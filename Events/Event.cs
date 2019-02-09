using GlLib.Entities;

namespace GlLib.Events
{
    public class EventBus
    {
        public static event EntitySpawnEvent EntitySpawn = (sender, entity) => false;
        public static event EntityDespawnEvent EntityDespawn = (sender, entity) => false;
        public static event EntityEnteredChunkEvent EntityEnteredChunk = (sender, entity) => false;
        public static event EntityLeftChunkEvent EntityLeftChunk = (sender, entity) => false;
        public static event EntityUpdateEvent EntityUpdate = (sender, entity) => false;

        public static bool OnEntitySpawn(Entity e)
        {
            return EntitySpawn(e, e);
        }

        public static bool OnEntityDespawn(Entity e)
        {
            return EntityDespawn(e, e);
        }

        public static bool OnEntityEnteredChunk(Entity e)
        {
            return EntityEnteredChunk(e, e);
        }

        public static bool OnEntityLeftChunk(Entity e)
        {
            return EntityLeftChunk(e, e);
        }

        public static bool OnEntityUpdate(Entity e)
        {
            return EntityUpdate(e, e);
        }
    }

    public delegate bool EntitySpawnEvent(object sender, Entity entity);

    public delegate bool EntityDespawnEvent(object sender, Entity entity);

    public delegate bool EntityEnteredChunkEvent(object sender, Entity entity);

    public delegate bool EntityLeftChunkEvent(object sender, Entity entity);

    public delegate bool EntityUpdateEvent(object sender, Entity entity);

}
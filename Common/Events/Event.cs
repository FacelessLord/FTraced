using System;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Common.Events
{
    public class EventBus
    {
        public static event EntitySpawnEvent EntitySpawn = (_sender, _entity) => false;
        public static event EntityDespawnEvent EntityDespawn = (_sender, _entity) => false;
        public static event EntityEnteredChunkEvent EntityEnteredChunk = (_sender, _entity) => false;
        public static event EntityLeftChunkEvent EntityLeftChunk = (_sender, _entity) => false;
        public static event EntityUpdateEvent EntityUpdate = (_sender, _entity) => false;

        public static Func<Entity, Entity, bool> identity = (_sender, _entity) => false;

        public static void Init()
        {
            SidedConsole.WriteLine("EventBus initialized");
        }
        
        public static bool OnEntitySpawn(Entity _e)
        {
            return EntitySpawn(_e, _e);
        }

        public static bool OnEntityDespawn(Entity _e)
        {
            return EntityDespawn(_e, _e);
        }

        public static bool OnEntityEnteredChunk(Entity _e)
        {
            return EntityEnteredChunk(_e, _e);
        }

        public static bool OnEntityLeftChunk(Entity _e)
        {
            return EntityLeftChunk(_e, _e);
        }

        public static bool OnEntityUpdate(Entity _e)
        {
            return EntityUpdate(_e, _e);
        }
    }

    public delegate bool EntitySpawnEvent(object _sender, Entity _entity);

    public delegate bool EntityDespawnEvent(object _sender, Entity _entity);

    public delegate bool EntityEnteredChunkEvent(object _sender, Entity _entity);

    public delegate bool EntityLeftChunkEvent(object _sender, Entity _entity);

    public delegate bool EntityUpdateEvent(object _sender, Entity _entity);
}
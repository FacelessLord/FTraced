using System;
using GlLib.Common.Entities;
using GlLib.Common.Io;

namespace GlLib.Common.Events
{
    public class EventBus
    {
        public static Func<Entity, Entity, bool> identity = (_sender, _entity) => false;
        public static event EntityEvent EntitySpawn = (_sender, _entity) => false;
        public static event EntityEvent EntityDespawn = (_sender, _entity) => false;
        public static event EntityEvent EntityEnteredChunk = (_sender, _entity) => false;
        public static event EntityEvent EntityLeftChunk = (_sender, _entity) => false;
        public static event EntityEvent EntityUpdate = (_sender, _entity) => false;
        public static event EntityEvent EntityDeath = (_sender, _entity) => false;

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

        public static bool OnEntityDeath(Entity _e)
        {
            return EntityDeath(_e, _e);
        }
    }

    public delegate bool EntityEvent(object _sender, Entity _entity);
}
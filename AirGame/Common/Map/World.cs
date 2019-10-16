using System;
using System.Linq;
using System.Net.Json;
using GlLib.Common.Entities;
using GlLib.Common.Events;
using GlLib.Common.Io;
using GlLib.Utils.Collections;
using GlLib.Utils.Math;

namespace GlLib.Common.Map
{
    public class World
    {
        public Chunk[,] chunks;
        public ThreadSafeList<(Entity e, Chunk chk)> entityAddQueue = new ThreadSafeList<(Entity e, Chunk chk)>();

        public ThreadSafeList<(Entity e, Chunk chk)> entityRemoveQueue = new ThreadSafeList<(Entity e, Chunk chk)>();
        public int height;

        public JsonObjectCollection jsonObj;

        public string mapName;

        public int width;
        public int worldId;

        public World(string _mapName, int _worldId, bool _fromStash = false)
        {
            FromStash = _fromStash;
            mapName = _mapName;
            worldId = _worldId;
        }

        public int MaxEntityCount { get; } = 200;
        public int EntityCount { get; private set; }

        public bool FromStash { get; }

        public Chunk this[int _i, int _j]
        {
            get => chunks[_i, _j];
            set => chunks[_i, _j] = value;
        }

        public void Update()
        {
            foreach (var pair in entityRemoveQueue)
                lock (pair.chk.entities)
                {
                    pair.chk.entities.Remove(pair.e);
                }

            entityRemoveQueue.Clear();
            foreach (var pair in entityAddQueue)
                lock (pair.chk.entities)
                {
                    pair.chk.entities.Add(pair.e);
                }

            entityAddQueue.Clear();
            lock (chunks)
            {
                foreach (var chunk in chunks)
                    if (chunk.isLoaded)
                        chunk.Update();
            }

//            WorldManager.SaveWorld(this);
//            Proxy.GetServer().profiler.SetState(State.Loop);
        }

        public void SpawnEntity(Entity _e)
        {
            if (EntityCount < MaxEntityCount)
            {
                if (EventBus.OnEntitySpawn(_e)) return;

                _e.worldObj = this;

                if (_e.chunkObj is null)
                    _e.chunkObj = Entity.GetProjection(_e.Position, this);
                lock (_e.chunkObj.entities)
                {
                    _e.chunkObj.entities.Add(_e); //todo entity null
                }

                EntityCount++;
                SidedConsole.WriteLine($"Entity {_e} spawned in world");
            }
            else
            {
                SidedConsole.WriteLine($"Entity count exceds maximum value({MaxEntityCount}). Couldn't spawn entity");
            }
        }

        public void SpawnEntityFromType(Type _type)
        {
            SpawnEntity((Entity) Activator.CreateInstance(_type));
        }


        public void ChangeEntityChunk(Entity _e, Chunk _old, Chunk _next)
        {
            entityRemoveQueue.Add((_e, _old));
            entityAddQueue.Add((_e, _next));
            _e.chunkObj = _next;
        }

        public ThreadSafeList<Entity> GetEntitiesWithinAaBb(AxisAlignedBb _aabb)
        {
            var chunks = new ThreadSafeList<Chunk>();

            var chkStartX = (_aabb.StartXi - 4) / 16;
            var chkStartY = (_aabb.StartYi - 4) / 16;
            var chkEndX = chkStartX + (_aabb.Width + 4) / 16 + 1;
            var chkEndY = chkStartY + (_aabb.Height + 4) / 16 + 1;
            for (var i = chkStartX; i <= chkEndX; i++)
            for (var j = chkStartY; j <= chkEndY; j++)
                if (i >= 0 && j >= 0 && i < width && j < height)
                {
                    var chk = this[i, j];
                    if (chk != null) chunks.Add(chk);
                }

            return chunks.SelectMany(_c => _c.entities)
                .ThreadSafeWhere(_entity =>
                    _entity.AaBb.IntersectsWithAt(ref _aabb, _entity.Position) && !_entity.noClip)
                .ToThreadSafeList();
        }

        public void DespawnEntity(Entity _entity)
        {
            entityRemoveQueue.Add((_entity, _entity.chunkObj));
            EntityCount--;
//            SidedConsole.WriteLine($"Entity {_entity} despawned in world");
        }
    }
}
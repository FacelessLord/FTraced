using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Json;
using System.Threading;
using GlLib.Common.Entities;
using GlLib.Common.Events;
using GlLib.Utils;

namespace GlLib.Common.Map
{
    public class World
    {
        public List<(Entity e, Chunk chk)> entityAddQueue = new List<(Entity e, Chunk chk)>();
        public List<(Entity e, Chunk chk)> entityRemoveQueue = new List<(Entity e, Chunk chk)>();

        public Chunk[,] chunks;

        public Mutex entityMutex = new Mutex();
        public int height;

        public JsonObjectCollection jsonObj;

        public string mapName;
        public bool FromStash { get; }

        public int width;
        public int worldId;

        public World(string _mapName, int _worldId, bool _fromStash=false)
        {
            FromStash = _fromStash; 
            mapName = _mapName;
            worldId = _worldId;
        }

        public Chunk this[int _i, int _j]
        {
            get => chunks[_i, _j];
            set => chunks[_i, _j] = value;
        }

        public void Update()
        {
            entityMutex.WaitOne();
            foreach (var pair in entityRemoveQueue) pair.chk.entities[pair.e.Position.z].Remove(pair.e);

            entityRemoveQueue.Clear();
            foreach (var pair in entityAddQueue) pair.chk.entities[pair.e.Position.z].Add(pair.e);

            entityAddQueue.Clear();
            lock (chunks)
                foreach (var chunk in chunks)
                    if (chunk.isLoaded)
                        chunk.Update();
            entityMutex.ReleaseMutex();
            //WorldManager.SaveWorld(this);
            //Proxy.GetServer().profiler.SetState(State.Loop);
        }

        public void SpawnEntity(Entity _e)
        {
            if (EventBus.OnEntitySpawn(_e)) return;

            entityMutex.WaitOne();
            _e.worldObj = this;

            if (_e.chunkObj is null)
                _e.chunkObj = Entity.GetProjection(_e.Position, this);
            _e.chunkObj.entities[_e.Position.z].Add(_e); //todo entity null
            entityMutex.ReleaseMutex();
            SidedConsole.WriteLine($"Entity {_e} spawned in world");
        }

        public void ChangeEntityChunk(Entity _e, Chunk _old, Chunk _next)
        {
            entityRemoveQueue.Add((_e, _old));
            entityAddQueue.Add((_e, _next));
            _e.chunkObj = _next;
        }

        public IEnumerable<Entity> GetEntitiesWithinAaBb(AxisAlignedBb _aabb)
        {
            var entities = new List<Entity>();

            var chunks = new List<Chunk>();

            var chkStartX = _aabb.StartXi / 16;
            var chkStartY = _aabb.StartYi / 16;
            var chkEndX = _aabb.EndXi / 16;
            var chkEndY = _aabb.EndYi / 16;

            for (var i = chkStartX; i <= chkEndX; i++)
            for (var j = chkStartY; j <= chkEndY; j++)
            {
                var chk = this[i, j];
                if (chk != null) chunks.Add(chk);
            }

            return chunks.SelectMany(_c => _c.entities).SelectMany(_el => _el);
        }

        public IEnumerable<Entity> GetEntitiesWithinAaBbAndHeight(AxisAlignedBb _aabb, int _height)
        {
            var entities = new List<Entity>();

            var chunks = new List<Chunk>();

            var chkStartX = _aabb.StartXi / 16;
            var chkStartY = _aabb.StartYi / 16;
            var chkEndX = _aabb.EndXi / 16;
            var chkEndY = _aabb.EndYi / 16;

            for (var i = chkStartX; i <= chkEndX; i++)
            for (var j = chkStartY; j <= chkEndY; j++)
            {
                if (i >= 0 && j >= 0 && i < width && j < _height)
                {
                    var chk = this[i, j];
                    if (chk != null) chunks.Add(chk);
                }
            }

            return chunks.SelectMany(_c => _c.entities[_height])
                .Where(_entity => _entity.GetAaBb().IntersectsWith(_aabb));
        }
    }
}
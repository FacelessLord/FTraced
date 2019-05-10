using System;
using System.Collections.Generic;
using System.Net.Json;
using System.Threading;
using GlLib.Common.Entities;
using GlLib.Common.Events;
using GlLib.Utils;

namespace GlLib.Common.Map
{
    public abstract class World
    {
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

        public void SpawnEntity(Entity _e)
        {
            if (EventBus.OnEntitySpawn(_e)) return;

            entityMutex.WaitOne();
            _e.worldObj = this;

            if (_e.chunkObj == null)
                _e.chunkObj = Entity.GetProjection(_e.Position, this);
            _e.chunkObj.entities[_e.Position.z].Add(_e); //todo entity null
            entityMutex.ReleaseMutex();
            SidedConsole.WriteLine($"Entity {_e} spawned in world");
        }

        public void LoadWorld()
        {
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (!this[i, j].isLoaded)
                    this[i, j].LoadChunk();
        }

        public List<Entity> GetEntitiesWithinAaBb(AxisAlignedBb _aabb)
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

            foreach (var chk in chunks)
            foreach (var height in chk.entities)
            foreach (var entity in height)
                entities.Add(entity);

            return entities;
        }

        public List<Entity> GetEntitiesWithinAaBbAndHeight(AxisAlignedBb _aabb, int _height)
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

            foreach (var chk in chunks)
            {
                var chkEntities = chk.entities[_height];
                foreach (var entity in chkEntities)
                    if (entity.GetAaBb().IntersectsWith(_aabb))
                        entities.Add(entity);
            }

            return entities;
        }
    }
}
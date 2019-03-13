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

        public int width;
        public int worldId;

        public World(string mapName, int worldId)
        {
            this.mapName = mapName;
            this.worldId = worldId;
        }

        public Chunk this[int i, int j]
        {
            get => chunks[i, j];
            set => chunks[i, j] = value;
        }

        public void SpawnEntity(Entity e)
        {
            if (EventBus.OnEntitySpawn(e)) return;

            entityMutex.WaitOne();
            if (e.chunkObj == null)
                e.chunkObj = Entity.GetProjection(e.Position, this);
            e.chunkObj.entities[e.Position.z].Add(e); //todo entity null
            entityMutex.ReleaseMutex();
            SidedConsole.WriteLine($"Entity {e} spawned in world");
        }

        public void LoadWorld()
        {
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (!this[i, j].isLoaded)
                    this[i, j].LoadChunk(i, j);
        }

        public List<Entity> GetEntitiesWithinAaBb(AxisAlignedBb aabb)
        {
            var entities = new List<Entity>();

            var chunks = new List<Chunk>();

            var chkStartX = aabb.StartXi / 16;
            var chkStartY = aabb.StartYi / 16;
            var chkEndX = aabb.EndXi / 16;
            var chkEndY = aabb.EndYi / 16;

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

        public List<Entity> GetEntitiesWithinAaBbAndHeight(AxisAlignedBb aabb, int height)
        {
            var entities = new List<Entity>();

            var chunks = new List<Chunk>();

            var chkStartX = aabb.StartXi / 16;
            var chkStartY = aabb.StartYi / 16;
            var chkEndX = aabb.EndXi / 16;
            var chkEndY = aabb.EndYi / 16;

            for (var i = chkStartX; i <= chkEndX; i++)
            for (var j = chkStartY; j <= chkEndY; j++)
            {
                var chk = this[i, j];
                if (chk != null) chunks.Add(chk);
            }

            foreach (var chk in chunks)
            {
                var chkEntities = chk.entities[height];
                foreach (var entity in chkEntities)
                    if (entity.GetAaBb().IntersectsWith(aabb))
                        entities.Add(entity);
            }

            return entities;
        }

        public virtual void SaveEntitiesToNbt(NbtTag tag)
        {
            var i = 0;
            var entityTag = new NbtTag();

            entityMutex.WaitOne();
            foreach (var chunk in chunks)
            foreach (var level in chunk.entities)
            foreach (var entity in level)
            {
                var localTag = new NbtTag();
                entity.SaveToNbt(localTag);
                entityTag.AppendTag(localTag, "Entity" + i);
                i++;
            }

            entityMutex.ReleaseMutex();

            entityTag.SetInt("EntityCount", i);
            tag.AppendTag(entityTag, "Entities");
        }

        public void LoadEntitiesFromNbt(NbtTag tag)
        {
            var entityTag = tag.RetrieveTag("Entities");
            var entityCount = entityTag.GetInt("EntityCount");

            entityMutex.WaitOne();
            for (var i = 0; i < entityCount; i++)
            {
                var entity = new Entity();
                entity.LoadFromNbt(entityTag.RetrieveTag("Entity" + i));
            }

            entityMutex.ReleaseMutex();
        }
    }
}
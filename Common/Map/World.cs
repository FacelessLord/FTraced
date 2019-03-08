using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Json;
using System.Threading;
using GlLib.Client.Input;
using GlLib.Common.Entities;
using GlLib.Common.Events;
using GlLib.Common.Packets;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Map
{
    public class World
    {
        public Chunk[,] chunks;
        public List<(Entity e, Chunk chk)> entityAddQueue = new List<(Entity e, Chunk chk)>();
        public List<(Entity e, Chunk chk)> entityRemoveQueue = new List<(Entity e, Chunk chk)>();

        public Mutex entityMutex = new Mutex();

        public int height;
        public JsonObjectCollection jsonObj;
        public string mapName;

        public ConcurrentDictionary<string, Player> players = new ConcurrentDictionary<string, Player>();

        public int width;

        public int worldId;

        public World(string mapName, int worldId)
        {
            this.mapName = mapName;
            this.worldId = worldId;
        }

        public Chunk this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= width)
                    return null;
                if (j < 0 || j >= height)
                    return null;
                return chunks[i, j];
            }
            set
            {
                if (i < 0 || i > width)
                    return;
                if (j < 0 || j > height)
                    return;
                chunks[i, j] = value;
            }
        }

        public void LoadWorld()
        {
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (!this[i, j].isLoaded)
                    this[i, j].LoadChunk(this, i, j);
        }

        public void SaveWorld()
        {
            var objects = new List<JsonObject>();
            objects.Add(new JsonNumericValue("Width", width));
            objects.Add(new JsonNumericValue("Height", height));
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                objects.Add(this[i, j].SaveChunk(this, i, j));

            var mainColl = new JsonObjectCollection(objects);

            var fs = File.OpenWrite(mapName);
            TextWriter tw = new StreamWriter(fs);
            mainColl.WriteTo(tw);
            tw.Flush();
            fs.Flush();
            tw.Close();
            fs.Close();
        }

        public void LoadWorldMap(JsonObjectCollection mapJson)
        {
            //todo
        }

        public void SpawnEntity(Entity e)
        {
            if (EventBus.OnEntitySpawn(e)) return;

            if (e is Player p)
                players.TryAdd(p.nickname, p);
            if (e.chunkObj == null)
                e.chunkObj = Entity.GetProjection(e.position, this);
            e.chunkObj.entities[e.position.z].Add(e);//todo entity null
            SidedConsole.WriteLine($"Entity {e} spawned in world");
        }

        public void SetBlockAt(TerrainBlock block, int x, int y)
        {
            var blockX = x % 16;
            var blockY = y % 16;
            var chunkX = x / 16;
            var chunkY = y / 16;

            var chunk = this[chunkX, chunkY];
            chunk[blockX, blockY] = block;
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

        public void Render(int x, int y)
        {
            var xAxis = new PlanarVector(Chunk.BlockWidth / 2, Chunk.BlockHeight / 2);
            var yAxis = new PlanarVector(Chunk.BlockWidth / 2, -Chunk.BlockHeight / 2);
            GL.PushMatrix();
            GL.Translate(-Math.Max(width, height) * Chunk.BlockWidth * 5, 0, 0);
            for (var i = 0; i < width; i++)
            for (var j = width - 1; j >= 0; j--)
                if (this[i + x, j + y].isLoaded)
                    this[i + x, j + y].RenderChunk(i, j, xAxis, yAxis);

            for (var i = 0; i < width; i++)
            for (var j = width - 1; j >= 0; j--)
                if (this[i + x, j + y].isLoaded)
                    foreach (var level in this[i + x, j + y].entities)
                    foreach (var entity in level)
                    {
                        var coord = xAxis * (entity.position.x - 8) + yAxis * (entity.position.y - 8);
                        GL.PushMatrix();

                        GL.Translate(coord.x, coord.y, 0);
                        entity.Render(xAxis, yAxis);
                        GL.PopMatrix();
                    }

            GL.PopMatrix();
        }

        public void Update()
        {
            foreach (var pair in entityRemoveQueue) pair.chk.entities[pair.e.position.z].Remove(pair.e);

            entityRemoveQueue.Clear();
            foreach (var pair in entityAddQueue) pair.chk.entities[pair.e.position.z].Add(pair.e);

            entityAddQueue.Clear();
            foreach (var player in players.Values)
            {
//                SidedConsole.WriteLine(player._nickname);
                player.Update();
            }

            entityMutex.WaitOne();
            foreach (var chunk in chunks)
                if (chunk.isLoaded)
                    chunk.Update();

            entityMutex.ReleaseMutex();
        }

        public void ChangeEntityChunk(Entity e, Chunk next)
        {
            entityRemoveQueue.Add((e, _chunkObj: e.chunkObj));
            entityAddQueue.Add((e, next));
            e.chunkObj = next;
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
            entityMutex.WaitOne();

            var entityTag = tag.RetrieveTag("Entities");
            var entityCount = entityTag.GetInt("EntityCount");
            for (var i = 0; i < entityCount; i++)
            {
                var entity = new Entity();
                entity.LoadFromNbt(entityTag.RetrieveTag("Entity" + i), this);
            }

            entityMutex.ReleaseMutex();
        }
    }
}
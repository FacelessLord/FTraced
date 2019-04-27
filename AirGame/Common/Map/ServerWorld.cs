using System.Collections.Generic;
using System.IO;
using System.Net.Json;
using GlLib.Common.Entities;

namespace GlLib.Common.Map
{
    public class ServerWorld : World
    {
        public List<(Entity e, Chunk chk)> entityAddQueue = new List<(Entity e, Chunk chk)>();
        public List<(Entity e, Chunk chk)> entityRemoveQueue = new List<(Entity e, Chunk chk)>();

        public ServerWorld(string _mapName, int _worldId) : base(_mapName, _worldId)
        {
        }

        public void SaveWorldEntities()
        {
            var mainColl = GetWorldEntitiesJson();

            var fs = File.OpenWrite("maps/" + mapName + "_entities.json");
            TextWriter tw = new StreamWriter(fs);
            mainColl.WriteTo(tw);
            tw.Flush();
            fs.Flush();
            tw.Close();
            fs.Close();
        }

        public JsonObjectCollection GetWorldEntitiesJson()
        {
            var objects = new List<JsonObject>();
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                objects.Add(this[i, j].SaveChunkEntities());

            return new JsonObjectCollection(objects);
        }

        public void Update()
        {
            foreach (var pair in entityRemoveQueue) pair.chk.entities[pair.e.Position.z].Remove(pair.e);

            entityRemoveQueue.Clear();
            foreach (var pair in entityAddQueue) pair.chk.entities[pair.e.Position.z].Add(pair.e);

            entityAddQueue.Clear();
            foreach (var chunk in chunks)
                if (chunk.isLoaded)
                    chunk.Update();

            SaveWorldEntities();
        }

        public void ChangeEntityChunk(Entity _e, Chunk _old, Chunk _next)
        {
            entityRemoveQueue.Add((_e, _old));
            entityAddQueue.Add((_e, _next));
            _e.chunkObj = _next;
        }
    }
}
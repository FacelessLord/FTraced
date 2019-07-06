using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Json;
using GlLib.Common.Entities;
using GlLib.Common.Io;
using GlLib.Utils;

namespace GlLib.Common.Map
{
    public static class WorldManager
    {
        private static Player GetPlayer => Proxy.GetClient().player;

        public static void SaveWorld(World _world)
        {
            Proxy.GetServer().profiler.SetState(State.SavingWorld);
            SaveWorldEntities(_world);
            SaveChunks(_world);
        }

        private static void SaveWorldEntities(World _world)
        {
            var mainColl = GetEntitiesJson(_world);
            File.WriteAllText("maps/" + _world.mapName + "_entities.json", mainColl + "");
        }

        private static JsonObjectCollection GetEntitiesJson(World _world)
        {
            var objects = new List<JsonObject>();
            for (var i = 0; i < _world.width; i++)
            for (var j = 0; j < _world.height; j++)
            {
                var collection = _world[i, j].SaveChunkEntities();
                objects.AddRange(collection);
            }

            return new JsonObjectCollection(objects);
        }

        public static void LoadWorld(World _world)
        {
            var entityJson = ReadEntities(_world);
            LoadChunks(_world, ReadWorldJson(_world), entityJson == null);
            if (entityJson != null) LoadEntities(_world, entityJson);
        }

        public static JsonObjectCollection ReadWorldJson(World _world)
        {
            var worldJson = File.ReadAllText("maps/" + _world.mapName + ".json");
            var parser = new JsonTextParser();
            var obj = parser.Parse(worldJson);
            return (JsonObjectCollection) obj;
        }

        private static JsonObjectCollection ReadEntities(World _world)
        {
            try
            {
                if (File.Exists("maps/" + _world.mapName + "_entities.json"))
                {
                    var fs = File.ReadAllText("maps/" + _world.mapName + "_entities.json");
                    var parser = new JsonTextParser();
                    return (JsonObjectCollection) parser.Parse(fs);
                }
            }
            catch (FormatException e)
            {
                SidedConsole.WriteErrorLine("There something wrong with your entity file.\n" +
                                            "It can be result of death of all entities");
            }

            return null;
        }

        private static void LoadChunks(World _world, JsonObjectCollection _worldCollection, bool loadEntities)
        {
            _world.jsonObj = _worldCollection;
            _world.width = (int) ((JsonNumericValue) _worldCollection[0]).Value;
            _world.height = (int) ((JsonNumericValue) _worldCollection[1]).Value;

            _world.chunks = new Chunk[_world.width, _world.height];

            for (var i = 0; i < _world.width; i++)
            for (var j = 0; j < _world.height; j++)
                _world[i, j] = new Chunk(_world, i, j);


            _world.jsonObj.Where(_o => _o is JsonObjectCollection).ToList().ForEach(_o =>
            {
                var name = _o.Name;
                var parts = name.Split(',');
                var i = int.Parse(parts[0].Trim());
                var j = int.Parse(parts[1].Trim());
                if (!_world[i, j].isLoaded)
                    _world[i, j].LoadFromJson((JsonObjectCollection) _o, loadEntities);
            });
        }

        private static void LoadEntities(World _world, JsonObjectCollection _entityCollection)
        {
            foreach (var entityJson in _entityCollection)
                if (entityJson != null)
                {
                    if (_world.FromStash)
                        break;
                    //TODO

                    var entity = Proxy.GetRegistry().GetEntityFromJson(entityJson as JsonObjectCollection);
                    _world.SpawnEntity(entity);
                }
        }

        public static void SaveChunks(World _world)
        {
            var objJson = new JsonObjectCollection();
            objJson.Add(new JsonNumericValue("Width", _world.width));
            objJson.Add(new JsonNumericValue("Height", _world.height));
            foreach (var chunk in GetPlayer.worldObj.chunks)
            {
                if (!chunk.isLoaded) continue;
                objJson.Add(chunk._unloadChunk(GetPlayer.worldObj, chunk.chunkX, chunk.chunkY));
            }

            if (!Directory.Exists(@"Saves")) Directory.CreateDirectory("Saves");

            var filePath = Proxy.GetServer().MachineTime
                               .ToString()
                               .Replace(":", "-") + ".json";

            if (!File.Exists(@"Saves/" + filePath))
                File.Create(@"Saves/" + filePath).Close();


            using (var file = File.CreateText(@"Saves/" + filePath))
            {
                objJson.WriteTo(file);
            }
        }
    }
}
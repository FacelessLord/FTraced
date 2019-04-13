using System.IO;
using System.Linq;
using System.Net.Json;
using GlLib.Client;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class ChunkEntitiesPacket : Packet
    {
        private string _mapName;
        private int _worldId;
        private int _x;
        private int _y;
        private string _entityJson;

        public ChunkEntitiesPacket()
        {
            _worldId = 0;
        }

        public ChunkEntitiesPacket(string mapName, int worldId, int x, int y)
        {
            _worldId = worldId;
            _mapName = mapName;
            _x = x;
            _y = y;
            ServerWorld world = Proxy.GetServer().GetWorldById(_worldId);
            _entityJson = world[x, y].SaveChunkEntities().ToString();

//            _worldJson = File.ReadAllText("maps/" + _mapName + "_entities.json");
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("MapName", _mapName);
            tag.SetString("EntityJson", _entityJson);
            tag.SetInt("WorldId", _worldId);
            tag.SetInt("ChunkX", _x);
            tag.SetInt("ChunkY", _y);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _mapName = tag.GetString("MapName");
            _entityJson = tag.GetString("EntityJson");
            _worldId = tag.GetInt("WorldId");
            _x = tag.GetInt("ChunkX");
            _y = tag.GetInt("ChunkY");
        }

        public override void OnClientReceive(ClientService client)
        {
            client.CurrentWorld = new ClientWorld(_mapName, _worldId);
            var parser = new JsonTextParser();
            var mainCollection = (JsonObjectCollection) parser.Parse(_entityJson);
            JsonObject chunkColl = mainCollection.First(e => e.Name == $"{_x}, {_y}");
            
            if (chunkColl != null && chunkColl is JsonObjectCollection entityList)
            {
                WorldManager.LoadEntitiesAtChunk(client.CurrentWorld, _x, _y, entityList);
            }

            base.OnClientReceive(client);
        }
    }
}
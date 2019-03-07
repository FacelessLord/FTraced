using System.IO;
using System.Net.Json;
using GlLib.Client;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class WorldMapPacket : Packet
    {
        private string _mapName;
        private int _worldId;
        private string _worldJson;

        public WorldMapPacket()
        {
            _worldId = 0;
        }

        public WorldMapPacket(string mapName, int worldId)
        {
            _worldId = worldId;
            _mapName = mapName;
            _worldJson = File.ReadAllText("maps/" + _mapName + ".json");
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("MapName", _mapName);
            tag.SetString("WorldJson", _worldJson);
            tag.SetInt("WorldId", _worldId);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _mapName = tag.GetString("MapName");
            _worldJson = tag.GetString("WorldJson");
            _worldId = tag.GetInt("WorldId");
        }

        public override void OnClientReceive(ClientService client)
        {

            client.CurrentWorld = new World(_mapName, _worldId);
            var parser = new JsonTextParser();
            var obj = parser.Parse(_worldJson);
            var mainCollection = (JsonObjectCollection) obj;
            WorldManager.LoadWorld(client.CurrentWorld, mainCollection);
            base.OnClientReceive(client);
        }
    }
}
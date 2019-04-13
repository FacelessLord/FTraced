using System.Net.Json;
using GlLib.Client;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class SyncPacket : Packet
    {
        private NbtTag _playerTag;
        private string _worldJson;

        public SyncPacket()
        {
        }

        public SyncPacket(ServerWorld world, Player player)
        {
            _playerTag = new NbtTag();
            player.SaveToNbt(_playerTag);
            _worldJson = world.GetWorldEntitiesJson().ToString();
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("WorldJson", _worldJson);
            tag.AppendTag(_playerTag, "Player");
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _worldJson = tag.GetString("WorldJson");
            _playerTag = tag.RetrieveTag("Player");
        }

        public override void OnClientReceive(ClientService client)
        {
            var clientService = client;
            var parser = new JsonTextParser();
            var entityCollection = (JsonObjectCollection) parser.Parse(_worldJson);
            WorldManager.LoadEntities(clientService.CurrentWorld,entityCollection);
            
            clientService.player = new Player();//todo ClientPlayer
            clientService.player.LoadFromNbt(_playerTag);
        }

        public override bool RequiresReceiveMessage()
        {
            return false;
        }
    }
}
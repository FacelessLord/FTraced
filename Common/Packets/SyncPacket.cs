using GlLib.Client;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class SyncPacket : Packet
    {
        private NbtTag _playerTag;
        private NbtTag _worldTag;

        public SyncPacket()
        {
        }

        public SyncPacket(ServerWorld world, Player player)
        {
            _playerTag = new NbtTag();
            player.SaveToNbt(_playerTag);
            _worldTag = new NbtTag();
            if (world != null)
                world.SaveEntitiesToNbt(_worldTag);
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.AppendTag(_worldTag, "World");
            tag.AppendTag(_playerTag, "Player");
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _worldTag = tag.RetrieveTag("World");
            _playerTag = tag.RetrieveTag("Player");
        }

        public override void OnClientReceive(ClientService client)
        {
            var clientService = client;
            clientService.CurrentWorld.LoadEntitiesFromNbt(_worldTag);
            clientService.player = new Player();
            clientService.player.LoadFromNbt(_playerTag);
        }

        public override bool RequiresReceiveMessage()
        {
            return false;
        }
    }
}
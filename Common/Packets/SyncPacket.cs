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
        
        public SyncPacket(World world, Player player)
        {
            _playerTag = new NbtTag();
            player.SaveToNbt(_playerTag);
            _worldTag = new NbtTag();
            if(world != null)
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

        public override void OnClientReceive(SideService client)
        {
            ClientService clientService = (ClientService) client;
            clientService._currentWorld.LoadEntitiesFromNbt(_worldTag);
            clientService._player = new Player();
            clientService._player.LoadFromNbt(_playerTag, clientService._currentWorld);

        }

        public override bool RequiresReceiveMessage()
        {
            return false;
        }
    }
}
using GlLib.Client;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class SyncPacket : Packet
    {
        private World _world;
        private Player _player;

        public SyncPacket(World world, Player player)
        {
            (_world, _player) = (world, player);
        }

        public override void WriteToNbt(NbtTag tag)
        {
            NbtTag worldTag = new NbtTag();
            _world.SaveToNbt(worldTag);

            NbtTag playerTag = new NbtTag();
            _player.SaveToNbt(playerTag);

            tag.AppendTag(worldTag, "World");
            tag.AppendTag(playerTag, "Player");
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            NbtTag worldTag = tag.RetrieveTag("World");
            _world = World.LoadFromNbt(worldTag);
            NbtTag playerTag = tag.RetrieveTag("Player");
            _player = new Player();
            _player.LoadFromNbt(playerTag);
        }

        public override void OnClientReceive(ClientService client)
        {
            client._currentWorld = _world;
        }
    }
}
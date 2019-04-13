using GlLib.Common;
using GlLib.Common.Packets;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Client.Packets
{
    public class ChunkEntitiesRequest : Request
    {
        private int _worldId;
        private int _x;
        private int _y;

        public ChunkEntitiesRequest()
        {
        }

        public ChunkEntitiesRequest(ClientService client, int worldId, int x, int y) : base(client)
        {
            (_worldId, _x, _y) = (worldId, x, y);
        }

        public override void WriteToNbt(NbtTag tag)
        {
            base.WriteToNbt(tag);
            tag.SetInt("WorldId", _worldId);
            tag.SetInt("ChunkX", _x);
            tag.SetInt("ChunkY", _y);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            base.ReadFromNbt(tag);
            _worldId = tag.GetInt("WorldId");
            _x = tag.GetInt("ChunkX");
            _y = tag.GetInt("ChunkY");
        }

        public override void OnServerReceive(ServerInstance server)
        {
            base.OnServerReceive(server);
            var worldName = server.GetWorldName(_worldId);
            var packet = new ChunkEntitiesPacket(worldName, _worldId, _x, _y);
            Proxy.SendPacketToPlayer(playerNickname, packet);
        }
    }
}
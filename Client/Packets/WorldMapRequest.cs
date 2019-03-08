using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class WorldMapRequest : Request
    {
        private int _worldId;

        public WorldMapRequest(){}
        
        public WorldMapRequest(ClientService client, int worldId) : base(client)
        {
            _worldId = worldId;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            base.WriteToNbt(tag);
            tag.SetInt("WorldId", _worldId);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            base.ReadFromNbt(tag);
            _worldId = tag.GetInt("WorldId");
        }

        public override void OnServerReceive(ServerInstance server)
        {
            base.OnServerReceive(server);
            string worldName = server.GetWorldName(_worldId);
            WorldMapPacket packet = new WorldMapPacket(worldName, _worldId);
            Proxy.SendPacketToPlayer(playerNickname, packet);
        }
    }
}
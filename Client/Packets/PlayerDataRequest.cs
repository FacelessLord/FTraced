using GlLib.Client;
using GlLib.Server;

namespace GlLib.Common.Packets
{
    public class PlayerDataRequestPacket : Request
    {
        public PlayerDataRequestPacket()
        {
        }

        public PlayerDataRequestPacket(ClientService client) : base(client)
        {
        }

        public override void OnServerReceive(ServerInstance server)
        {
            var playerDataPacket =
                new PlayerDataPacket(server.GetDataFor(playerNickname, password));
            Proxy.SendPacketToPlayer(playerNickname, playerDataPacket);
        }
    }
}
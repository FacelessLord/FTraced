using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

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
                new PlayerDataPacket(((ServerInstance) server).GetDataFor(playerNickname, password));
            Proxy.SendPacketToPlayer(playerNickname, playerDataPacket);
        }
    }
}
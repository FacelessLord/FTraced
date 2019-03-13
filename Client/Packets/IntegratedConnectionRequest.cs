using GlLib.Client;
using GlLib.Server;

namespace GlLib.Common.Packets
{
    public class IntegratedConnectionRequestPacket : Request
    {
        public ClientService client;

        public IntegratedConnectionRequestPacket()
        {
        }

        public IntegratedConnectionRequestPacket(ClientService client) : base(client)
        {
            this.client = client;
        }

        public override void OnServerReceive(ServerInstance server)
        {
            server.ConnectClient(client);

            var connectionPacket =
                new ConnectionEstablishedPacket(server.serverId);

            Proxy.SendPacketToPlayer(client.nickName, connectionPacket);
        }
    }
}
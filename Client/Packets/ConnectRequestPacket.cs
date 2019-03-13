using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class ConnectRequestPacket : Packet
    {
        public string password;

        public string playerNickname;

        public ConnectRequestPacket()
        {
        }

        public ConnectRequestPacket(string nickname, string password)
        {
            playerNickname = nickname;
            this.password = password;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("Nickname", playerNickname);
            tag.SetString("Password", password);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            playerNickname = tag.GetString("Nickname");
            password = tag.GetString("Password");
        }

        public override void OnServerReceive(ServerInstance server)
        {
            var client = new ClientService(playerNickname, password);
            server.ConnectClient(client);

            var connectionPacket =
                new ConnectionEstablishedPacket(server.serverId);

            Proxy.SendPacketToPlayer(playerNickname, connectionPacket);
        }
    }
}
using System;
using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class ConnectRequestPacket : Packet
    {
        public ConnectRequestPacket()
        {
        }

        public string _playerNickname;
        public string _password;

        public ConnectRequestPacket(string nickname, string password)
        {
            _playerNickname = nickname;
            _password = password;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("Nickname", _playerNickname);
            tag.SetString("Password", _password);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _playerNickname = tag.GetString("Nickname");
            _password = tag.GetString("Password");
        }

        public override void OnServerReceive()
        {
            ClientService client = new ClientService(_playerNickname, _password);
            ServerInstance.ConnectClient(client);

            ConnectionEstablishedPacket connectionPacket =
                new ConnectionEstablishedPacket(ServerInstance._serverId);

            Proxy.SendPacketToPlayer(_playerNickname, connectionPacket);
        }
    }
}
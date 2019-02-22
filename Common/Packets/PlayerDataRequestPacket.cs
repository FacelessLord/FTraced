using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class PlayerDataRequestPacket : Packet
    {
        public PlayerDataRequestPacket()
        {
        }

        public string _playerNickname;
        public string _password;

        public PlayerDataRequestPacket(string nickname, string password)
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

        public override void OnServerReceive(SideService server)
        {
            PlayerDataPacket playerDataPacket = 
                new PlayerDataPacket(((ServerInstance)server).GetDataFor(_playerNickname,_password));
            Proxy.SendPacketToPlayer(_playerNickname,playerDataPacket);
        }
    }
}
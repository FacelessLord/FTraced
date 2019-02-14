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
        
        public PlayerDataRequestPacket(string nickname)
        {
            _playerNickname = nickname;
        }
        
        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("Nickname",_playerNickname);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _playerNickname = tag.GetString("Nickname");
        }

        public override void OnServerReceive()
        {
            PlayerDataPacket playerDataPacket = new PlayerDataPacket(ServerInstance.GetDataFor(_playerNickname));
            Proxy.SendPacketToPlayer(_playerNickname,playerDataPacket);
        }
    }
}
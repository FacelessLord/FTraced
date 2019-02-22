using GlLib.Client;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class PlayerDataPacket : Packet
    {
        public PlayerDataPacket()
        {
        }

        public PlayerData _playerData;
        public PlayerDataPacket(PlayerData data)
        {
            _playerData = data;
        }
        
        public override void WriteToNbt(NbtTag tag)
        {
            _playerData.SaveToNbt(tag);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _playerData = PlayerData.LoadFromNbt(tag);
        }

        public override void OnClientReceive(SideService client)
        {
            ((ClientService)client)._player.Data = _playerData;
        }
    }
}
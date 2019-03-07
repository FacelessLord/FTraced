using GlLib.Client;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class PlayerDataPacket : Packet
    {
        public PlayerData playerData;

        public PlayerDataPacket()
        {
        }

        public PlayerDataPacket(PlayerData data)
        {
            playerData = data;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            playerData.SaveToNbt(tag);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            playerData = PlayerData.LoadFromNbt(tag);
        }

        public override void OnClientReceive(ClientService client)
        {
            client.player.Data = playerData;
        }
    }
}
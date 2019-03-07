using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class ConnectionEstablishedPacket : Packet
    {
        public int serverId;

        public ConnectionEstablishedPacket()
        {
        }

        public ConnectionEstablishedPacket(int serverId)
        {
            this.serverId = serverId;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetInt("ServerId", serverId);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            serverId = tag.GetInt("ServerId");
        }

        public override void OnClientReceive(ClientService client)
        {
            client.serverId = serverId;
        }
    }
}
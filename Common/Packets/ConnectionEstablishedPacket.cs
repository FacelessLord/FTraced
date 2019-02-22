using System;
using GlLib.Client;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class ConnectionEstablishedPacket : Packet
    {
        public ConnectionEstablishedPacket()
        {
        }

        public int _serverId;
        
        public ConnectionEstablishedPacket(int serverId)
        {
            _serverId = serverId;
        }
        
        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetInt("ServerId",_serverId);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _serverId = tag.GetInt("ServerId");
        }
        
        public override void OnClientReceive(ClientService client)
        {
            client._serverId = _serverId;
        }
    }
}
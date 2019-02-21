using System;
using GlLib.Client;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class ConnectedEstablishedPacket : Packet
    {
        public ConnectedEstablishedPacket()
        {
            _packetId = 1;
        }

        public int _serverId;
        
        public ConnectedEstablishedPacket(int serverId)
        {
            _serverId = serverId;
            _packetId = 1;
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
            Console.WriteLine("PacketReceived");
            client._serverId = _serverId;
        }
    }
}
using System;
using System.Collections.Generic;

namespace GlLib.Common.Packets
{
    public static class PacketRegistry
    {
        public static void Register()
        {
            RegisterPacket(new ConnectRequestPacket());
            RegisterPacket(new IntegratedConnectionRequestPacket());
            RegisterPacket(new ConnectionEstablishedPacket());
            RegisterPacket(new PlayerDataRequestPacket());
            RegisterPacket(new PlayerDataPacket());
            RegisterPacket(new KeyPressedPacket());
        }

        public static void RegisterPacket(Packet packet)
        {
            _packets.Add(packet.GetType().Name,GetNextId());
        }
        public static bool IsPacketRegistered(Packet packet)
        {
            return _packets.ContainsKey(packet.GetType().Name);
        }

        public static Dictionary<string,int> _packets = new Dictionary<string,int>();
        
        private static int LastId = 0;
        public static int GetNextId()
        {
            LastId++;
            return LastId-1;
        }

        public static int GetPacketId(Packet packet)
        {
            if(!IsPacketRegistered(packet))
                throw new ArgumentException($"Tried to Send Not registered Packet {packet.GetType().Name}");
            return _packets[packet.GetType().Name];
        }
    }
}
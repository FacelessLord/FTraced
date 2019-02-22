using System;
using System.Collections.Generic;
using GlLib.Common.Map;
using GlLib.Common.Packets;

namespace GlLib.Common.Registries
{
    public class PacketRegistry
    {
        public void Register()
        {
            RegisterPacket(new ConnectRequestPacket());
            RegisterPacket(new IntegratedConnectionRequestPacket());
            RegisterPacket(new ConnectionEstablishedPacket());
            RegisterPacket(new PlayerDataRequestPacket());
            RegisterPacket(new PlayerDataPacket());
            RegisterPacket(new KeyPressedPacket());
            RegisterPacket(new SyncPacket());
        }

        public  void RegisterPacket(Packet packet)
        {
            _packets.Add(packet.GetType().Name,GetNextId());
        }
        public bool IsPacketRegistered(Packet packet)
        {
            return _packets.ContainsKey(packet.GetType().Name);
        }

        public Dictionary<string,int> _packets = new Dictionary<string,int>();
        
        private int LastId = 0;
        public int GetNextId()
        {
            LastId++;
            return LastId-1;
        }

        public int GetPacketId(Packet packet)
        {
            if(!IsPacketRegistered(packet))
                throw new ArgumentException($"Tried to Send Not registered Packet {packet.GetType().Name}");
            return _packets[packet.GetType().Name];
        }
    }
}
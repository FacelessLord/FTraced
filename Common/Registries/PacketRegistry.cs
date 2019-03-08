using System;
using System.Collections.Generic;
using GlLib.Common.Packets;

namespace GlLib.Common.Registries
{
    public class PacketRegistry
    {
        private int _lastId;

        public Dictionary<string, int> packets = new Dictionary<string, int>();

        public void Register()
        {
            RegisterPacket(new ConnectRequestPacket());
            RegisterPacket(new IntegratedConnectionRequestPacket());
            RegisterPacket(new ConnectionEstablishedPacket());
            RegisterPacket(new PlayerDataRequestPacket());
            RegisterPacket(new PlayerDataPacket());
            RegisterPacket(new SyncPacket());
            RegisterPacket(new WorldMapRequest());;
            RegisterPacket(new WorldMapPacket());
            RegisterPacket(new KeyPressedPacket());
            RegisterPacket(new KeyUnpressedPacket());
        }

        public void RegisterPacket(Packet packet)
        {
            packets.Add(packet.GetType().Name, GetNextId());
        }

        public bool IsPacketRegistered(Packet packet)
        {
            return packets.ContainsKey(packet.GetType().Name);
        }

        public int GetNextId()
        {
            _lastId++;
            return _lastId - 1;
        }

        public int GetPacketId(Packet packet)
        {
            if (!IsPacketRegistered(packet))
                throw new ArgumentException($"Tried to Send Not registered Packet {packet.GetType().Name}");
            return packets[packet.GetType().Name];
        }
    }
}
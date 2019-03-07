using System.Collections.Generic;
using System.Threading;
using GlLib.Client;
using GlLib.Common;
using GlLib.Common.Packets;
using GlLib.Utils;

namespace GlLib.Server
{
    public abstract class PacketHandler
    {
        public volatile Queue<Packet> receivedPackets = new Queue<Packet>();

        public void ReceivePacket(Packet packet)
        {
//            SidedConsole.WriteLine("PacketSent");
            receivedPackets.Enqueue(packet);
        }

        public void StartPacketHandler()
        {
            var handlerThread = new Thread(() => PacketLoop());
            handlerThread.Name = "" + Proxy.GetSide() + "PacketHandler";
            handlerThread.Start();
        }

        public void PacketLoop()
        {
            SidedConsole.WriteLine("Handler Started");
            while (true)
            {
                while (receivedPackets.Count <= 0)
                {
                }

//                SidedConsole.WriteLine("PacketProcessed");
                HandlePacket(receivedPackets.Dequeue());
            }
        }

        public abstract void HandlePacket(Packet packet);
    }

    public class ServerPacketHandler : PacketHandler
    {
        private readonly ServerInstance _server;

        public ServerPacketHandler(SideService client)
        {
            _server = (ServerInstance) client;
        }

        public override void HandlePacket(Packet packet)
        {
            _server.HandlePacket(packet);
        }
    }


    public class ClientPacketHandler : PacketHandler
    {
        private readonly ClientService _client;

        public ClientPacketHandler(SideService client)
        {
            _client = (ClientService) client;
        }

        public override void HandlePacket(Packet packet)
        {
            _client.HandlePacket(packet);
        }
    }
}
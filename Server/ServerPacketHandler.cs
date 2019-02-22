using System;
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
        public volatile Queue<Packet> _receivedPackets = new Queue<Packet>();
        
        public void ReceivePacket(Packet packet)
        {
//            SidedConsole.WriteLine("PacketSent");
            _receivedPackets.Enqueue(packet);
        }

        public void StartPacketHandler()
        {
            Thread handlerThread = new Thread( () => PacketLoop());
            handlerThread.Name = ""+Proxy.GetSide()+"PacketHandler";
            handlerThread.Start();
        }

        public void PacketLoop()
        {
            SidedConsole.WriteLine("Handler Started");
            while (true)
            {    
                while (_receivedPackets.Count <= 0)
                {
                }
                
//                SidedConsole.WriteLine("PacketProcessed");
                HandlePacket(_receivedPackets.Dequeue());
            }
        }

        public abstract void HandlePacket(Packet packet);
    }

    public class ServerPacketHandler : PacketHandler
    {
        public override void HandlePacket(Packet packet)
        {
            ServerInstance.HandlePacket(packet);
        }
    }
    
    
    public class ClientPacketHandler : PacketHandler
    {
        private ClientService _client;

        public ClientPacketHandler(ClientService client)
        {
            _client = client;
        }

        public override void HandlePacket(Packet packet)
        {
            _client.HandlePacket(packet);
        }
    }
}
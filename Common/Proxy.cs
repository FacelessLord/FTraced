using System;
using GlLib.Common.Packets;
using GlLib.Utils;
using System.Linq;
using System.Threading;
using GlLib.Server;

namespace GlLib.Common
{
    public class Proxy
    {

        public static void SendPacketToPlayer(string nickName, Packet packet)
        {
            if (Config._isIntegratedServer)
            {
                foreach (var client in ServerInstance._clients)
                {
                    if (client._nickName == nickName)
                    {
                        client._packetHandler.ReceivePacket(packet);
                    }
                }
            }

            //todo not Integrated Server
        }

        public static void SendPacketToAllAround(PlanarVector pos, double range, Packet packet)
        {

            //todo not Integrated Server
        }

        public static void SendPacketToWorld(int worldId, Packet packet)
        {
            //todo not Integrated Server
        }

        public static void SendPacketToServer(Packet packet)
        {
            if (Config._isIntegratedServer)
            {
                ServerInstance._packetHandler.ReceivePacket(packet);
            }

            //todo not Integrated Server
        }
        
        public static void AwaitWhile(Func<bool> condition)
        {
            while (condition.Invoke()) //while condition is true
            {
            }
        }

        public static void Sync()
        {
            //todo send SyncPackage
        }

        public static Side GetSide()
        {
            string packetHandler = "PacketHandler";
            if (Thread.CurrentThread.Name == Side.Server.ToString() || Thread.CurrentThread.Name == Side.Server + packetHandler)
                return Side.Server;
            if (Thread.CurrentThread.Name == Side.Graphics.ToString())
                return Side.Graphics;
            if (Thread.CurrentThread.Name == Side.Client.ToString() || Thread.CurrentThread.Name == Side.Client +packetHandler)
                return Side.Client;
            return Side.Other;
        }
    }
}
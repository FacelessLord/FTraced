using System;
using System.Collections.Concurrent;
using GlLib.Common.Packets;
using GlLib.Utils;
using System.Linq;
using System.Threading;
using GlLib.Client;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Server;

namespace GlLib.Common
{
    public class Proxy
    {
        public static void SendPacketToPlayer(string nickName, Packet packet)
        {
            if (Config._isIntegratedServer)
            {
                foreach (var client in GetServer()._clients)
                {
                    if (client._nickName == nickName)
                    {
                        client._packetHandler.ReceivePacket(packet);
                    }
                }
            }

            //todo not Integrated Server
        }

        public static ConcurrentDictionary<string, SideService> _services =
            new ConcurrentDictionary<string, SideService>();

        public static ClientService GetClient()
        {
            SideService service = GetService();
            if (service is ClientService client)
            {
                return client;
            }
            throw new InvalidOperationException(SidedConsole.GetSidePrefix()+"Client doesn't exist in this context");
        }
        
        public static ServerInstance GetServer()
        {
            SideService service = GetService();
            if (service is ServerInstance server)
            {
                return server;
            }
            throw new InvalidOperationException(SidedConsole.GetSidePrefix()+"Server doesn't exist in this context");
        }

        public static ServerInstance _serverInstance;
        
        public static SideService GetService()
        {
            string key = GetSide().ToString();
            if (_services.ContainsKey(key))
                return _services[key];
            throw new InvalidOperationException(SidedConsole.GetSidePrefix()+"Tried to get non-existing side");
        }

        public static GameRegistry GetSideRegistry()
        {
            return GetService()._registry;
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
                _serverInstance._packetHandler.ReceivePacket(packet);
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
            foreach (var client in GetServer()._clients)
            {
                Player player = client._player;
                World world = client._currentWorld;

                SendPacketToPlayer(player._nickname, new SyncPacket(world, player));
            }
        }

        public static Side GetSide()
        {
            string packetHandler = "PacketHandler";
            if (Thread.CurrentThread.Name == Side.Server.ToString() ||
                Thread.CurrentThread.Name == Side.Server + packetHandler)
                return Side.Server;
            if (Thread.CurrentThread.Name == Side.Client.ToString() ||
                Thread.CurrentThread.Name == Side.Graphics.ToString() ||
                Thread.CurrentThread.Name == Side.Client + packetHandler)
                return Side.Client;
            return Side.Other;
        }

        public static void RegisterService(SideService sideService)
        {
            _services.TryAdd(GetSide().ToString(), sideService);
            SidedConsole.WriteLine(sideService._side+"-Side service registered");
        }
    }
}
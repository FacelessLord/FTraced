using System;
using System.Collections.Concurrent;
using System.Threading;
using GlLib.Client;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common
{
    public class Proxy
    {
        public static ConcurrentDictionary<string, SideService> services =
            new ConcurrentDictionary<string, SideService>();

        public static ServerInstance serverInstance;

        public static void SendPacketToPlayer(string nickName, Packet packet)
        {
            if (Config.isIntegratedServer)
                foreach (var client in GetServer().clients)
                    if (client.nickName == nickName)
                        client.packetHandler.ReceivePacket(packet);

            //todo not Integrated Server
        }

        public static ClientService GetClient()
        {
            var service = GetService();
            if (service is ClientService client) return client;
            throw new InvalidOperationException(SidedConsole.GetSidePrefix() + "Client doesn't exist in this context");
        }

        public static ServerInstance GetServer()
        {
            var service = GetService();
            if (service is ServerInstance server) return server;
            throw new InvalidOperationException(SidedConsole.GetSidePrefix() + "Server doesn't exist in this context");
        }

        public static SideService GetService()
        {
            var key = GetSide().ToString();
            if (services.ContainsKey(key))
                return services[key];
            throw new InvalidOperationException(SidedConsole.GetSidePrefix() + "Tried to get non-existing side");
        }

        public static GameRegistry GetSideRegistry()
        {
            return GetService().registry;
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
            if (Config.isIntegratedServer) serverInstance.packetHandler.ReceivePacket(packet);

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
            foreach (var client in GetServer().clients)
            {
                var player = client.player;
                var world = client.CurrentWorld;

                SendPacketToPlayer(player.nickname, new SyncPacket(world, player));
            }
        }

        public static Side GetSide()
        {
            var packetHandler = "PacketHandler";
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
            services.TryAdd(GetSide().ToString(), sideService);
            SidedConsole.WriteLine(sideService.side + "-Side service registered");
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Threading;
using GlLib.Client;
using GlLib.Common.Map;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common
{
    public class Proxy
    {
        public static ConcurrentDictionary<string, SideService> services =
            new ConcurrentDictionary<string, SideService>();

        public static ServerInstance serverInstance;
        public static ClientService clientInstance;

        public static ClientService GetClient()
        {
            return clientInstance;
        }

        public static ServerInstance GetServer()
        {
            return serverInstance;
        }
        public static GameRegistry GetRegistry()
        {
            return serverInstance.registry;
        }
        
        public static void AwaitWhile(Func<bool> _condition)
        {
            while (_condition.Invoke()) //while condition is true
            {
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

        public static void RegisterService(SideService _sideService)
        {
            services.TryAdd(GetSide().ToString(), _sideService);
            SidedConsole.WriteLine(_sideService.side + "-Side service registered");
            if (_sideService is ServerInstance server)
            {
                serverInstance = server;
            }
            if (_sideService is ClientService client)
            {
                clientInstance = client;
            }
        }
    }
}
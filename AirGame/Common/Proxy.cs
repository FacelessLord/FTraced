using System;
using System.Collections.Concurrent;
using System.Threading;
using GlLib.Client;
using GlLib.Client.Graphic;
using GlLib.Common.Map;
using GlLib.Common.Registries;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common
{
    public class Proxy
    {
        public static ConcurrentDictionary<string, SideService> services =
            new ConcurrentDictionary<string, SideService>();

        private static bool _exit;

        public static bool Exit
        {
            get => _exit;
            set => _exit = value;
        }

        private static ServerInstance _serverInstance;
        private static ClientService _clientInstance;
        private static GraphicWindow _gameWindow;
        private static readonly Profiler Profiler = new Profiler();

        public static ClientService GetClient()
        {
            return _clientInstance;
        }

        public static ServerInstance GetServer()
        {
            return _serverInstance;
        }
        public static GameRegistry GetRegistry()
        {
            return _serverInstance.registry;
        }

        public static GraphicWindow GetWindow()
        {
            return _gameWindow;
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
                _serverInstance = server;
            }
            if (_sideService is ClientService client)
            {
                _clientInstance = client;
            }
        }

        public static void RegisterWindow(GraphicWindow _window)
        {
            _gameWindow = _window;
        }
    }
}
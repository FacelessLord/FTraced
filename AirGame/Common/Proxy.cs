using System;
using System.Collections.Concurrent;
using System.Threading;
using GlLib.Client;
using GlLib.Client.Graphic;
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

        private static ServerInstance _serverInstance;
        private static ClientService _clientInstance;
        private static GraphicWindow _gameWindow;
        /// <summary>
        /// Registry used to work only on one Side (line in Map Editor)
        /// </summary>
        private static GameRegistry _registry;
        private static readonly Profiler Profiler = new Profiler();

        public static bool Exit
        {
            get => _exit;
            set
            {
                SidedConsole.WriteLine("Exiting...");
                GetWindow().Exit();
                _exit = value;
            }
        }

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
            return _registry ?? _serverInstance.registry;
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
            if (_sideService is ServerInstance server) _serverInstance = server;
            if (_sideService is ClientService client) _clientInstance = client;
        }

        public static void RegisterWindow(GraphicWindow _window)
        {
            _gameWindow = _window;
        }

        public static void RegisterRegistry(GameRegistry _gameRegistry)
        {
            _registry = _gameRegistry;
        }
    }
}
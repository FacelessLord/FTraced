using System;
using System.Linq;
using System.Threading;
using GlLib.Client;
using GlLib.Client.Api.Gui;
using GlLib.Client.Graphic;
using GlLib.Server;
using GlLib.Utils;
using GlLib.Common.Events;

namespace GlLib.Common
{
    public static class Core
    {
        public static void Main(string[] _args)
        {
            EventBus.Init();
            foreach (var arg in _args)
            {
                Console.WriteLine("Arguments: [" + _args.Aggregate((_a, _b) => _a + "," + _b) + "]");
                var argsParts = arg.Split("=");
                var (variableName, value) = (argsParts[0], argsParts[1]);
                Config.ProcessArgument(variableName, value);
            }

            GraphicWindow.RunWindow();
            SidedConsole.WriteLine("Core finished");
            // ClientService._instance.ConnectToIntegratedServer();
        }

        public static void StartWorld()
        {
            var server = new ServerInstance();
            Proxy.RegisterService(server);
            var serverThread = new Thread(() =>
            {
                server.Start();
                server.Loop();
                server.Exit();
            }) {Name = Side.Server.ToString()};

            var client = new ClientService(Config.playerName, Config.playerPassword);
            Proxy.RegisterService(client);
            var clientThread = new Thread(() =>
            {
                client.Start();
                client.Loop();
                client.Exit();
            }) {Name = Side.Client.ToString()};
            serverThread.Start();
            Proxy.AwaitWhile(() => server.profiler.state < State.Loop);
            clientThread.Start();
            Proxy.AwaitWhile(() => client.profiler.state < State.Loop);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using GlLib.Client;
using GlLib.Client.Graphic;
using GlLib.Common.Events;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common
{
    public static class Core
    {
        public static Profiler profiler = new Profiler();

        public static void Main(string[] _args)
        {
            profiler.SetState(State.CoreStarting);
            try
            {
                EventBus.Init();
                foreach (var arg in _args)
                {
                    Console.WriteLine("Arguments: [" + _args.Aggregate((_a, _b) => _a + "," + _b) + "]");
                    var argsParts = arg.Split("=");
                    Config.ProcessArgument(argsParts[0], argsParts[1]);
                }

                GraphicWindow.RunWindow();
                Proxy.AwaitWhile(() => profiler.state < State.MainMenu);
//                StartWorld();
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }

            SidedConsole.WriteLine("Core finished");
            // ClientService._instance.ConnectToIntegratedServer();
        }

        public static void StartWorld()
        {
            var server = new ServerInstance();
            var serverThread = new Thread(() =>
            {
                server.Start();
                server.Loop();
                server.Exit();
            }) {Name = Side.Server.ToString()};
            serverThread.Start();
            Proxy.AwaitWhile(() => server.profiler.state < State.Loop);

            var client = new ClientService(Config.playerName, Config.playerPassword);
            var clientThread = new Thread(() =>
            {
                client.Start();
                client.Loop();
                client.Exit();
            }) {Name = Side.Client.ToString()};
            clientThread.Start();
            Proxy.AwaitWhile(() => client.profiler.state < State.Loop);
        }
    }
}
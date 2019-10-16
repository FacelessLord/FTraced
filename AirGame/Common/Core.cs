using System;
using System.Linq;
using GlLib.Client;
using GlLib.Client.Graphic;
using GlLib.Common.Events;
using GlLib.Common.Io;
using GlLib.Server;

namespace GlLib.Common
{
    public static class Core
    {
        public static Profiler profiler = new Profiler();

        public static GameClient client = new GameClient();
        public static GameServer server = new GameServer();

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
            server.Start();
            client.Start();

            //TODO Start GameClient
        }

        public static void StopWorld(string _cause)
        {
            Proxy.GetClient().AskToStop(_cause);
            Proxy.AwaitWhile(() => Proxy.GetClient().profiler.state < State.Off);
            Proxy.GetServer().AskToStop(_cause);
            Proxy.AwaitWhile(() => Proxy.GetServer().profiler.state < State.Off);
            Proxy.GetWindow().serverStarted = false;
        }
    }
}
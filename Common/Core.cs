using System;
using System.Linq;
using System.Threading;
using GlLib.Client;
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
            var server = new ServerInstance();
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
            Proxy.AwaitWhile(() => server.state <= State.Starting);
            clientThread.Start();
            //todo Main Menu
            Proxy.AwaitWhile(() => client.state <= State.Starting);
            SidedConsole.WriteLine("Core finished");
            // ClientService._instance.ConnectToIntegratedServer();
        }
    }
}
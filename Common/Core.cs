using System;
using System.Linq;
using System.Threading;
using GlLib.Client;
using GlLib.Server;

namespace GlLib.Common
{
    internal static class Core
    {
        public static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine("Arguments: [" + args.Aggregate((a, b) => a + "," + b) + "]");
                var argsParts = arg.Split("=");
                var (variableName, value) = (argsParts[0], argsParts[1]);
                Config.ProcessArgument(variableName, value);
            }

            if (Config.isIntegratedServer)
            {
                var server = new ServerInstance();
                var serverThread = new Thread(() =>
                {
                    server.Start();
                    server.Loop();
                    server.Exit();
                }) {Name = Side.Server.ToString()};

                var client = new ClientService(Config.playerName, Config.playerPassword);
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
//                ClientService._instance.ConnectToIntegratedServer();
            }
        }
    }
}
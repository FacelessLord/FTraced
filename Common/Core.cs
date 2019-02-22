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
                Console.WriteLine("Arguments: ["+args.Aggregate((a,b) => a+","+b)+"]");
                string[] argsParts = arg.Split("=");
                (string variableName, string value) = (argsParts[0], argsParts[1]);
                Config.ProcessArgument(variableName, value);
            }

            if (Config._isIntegratedServer)
            {
                ServerInstance server = new ServerInstance();
                Thread serverThread = new Thread(() =>
                {
                    server.Start();
                    server.Loop();
                    server.Exit();
                });
                serverThread.Name = Side.Server.ToString();

                ClientService client = new ClientService(Config._playerName, Config._playerPassword);
                Thread clientThread = new Thread(() =>
                {
                    ClientService._instance.StartClient();
                    ClientService._instance.GameLoop();
                    ClientService._instance.ExitGame();
                });
                clientThread.Name = Side.Client.ToString();
                serverThread.Start();
                Proxy.AwaitWhile(() => ServerInstance._state <= State.Starting);
                clientThread.Start();
                //todo Main Menu
                Proxy.AwaitWhile(() => ClientService._instance._state <= State.Starting);
//                ClientService._instance.ConnectToIntegratedServer();
            }
        }
    }
}
using System.Threading;
using GlLib.Client;

namespace GlLib.Common
{
    public class Config
    {
        public static bool _isIntegratedServer = true;
        public static string _side = "both";
        public static bool IsClient => Thread.CurrentThread.Name == "Client";
        public static bool IsServer => Thread.CurrentThread.Name == "Server";
        public static string _playerName = "Noname";
        public static string _playerPassword = "";

        public static void ProcessArgument(string argument, string value)
        {
            switch (argument)
            {
                case "side":
                    switch (value)
                    {
                        case "client":
                            _isIntegratedServer = false;
                            _side = "client";
                            break;
                        case "server":
                            _isIntegratedServer = false;
                            _side = "server";
                            break;
                        case "both":
                            _isIntegratedServer = true;
                            _side = "both";
                            break;
                    }

                    break;
                case "username":
                    _playerName = value;
                    break;
                case "password":
                    _playerPassword = value;
                    break;
            }
        }
    }
}
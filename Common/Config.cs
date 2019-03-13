namespace GlLib.Common
{
    public class Config
    {
        public static bool isIntegratedServer = true;
        public static string side = "both";
        public static string playerName = "Noname";
        public static string playerPassword = "";

        public static void ProcessArgument(string argument, string value)
        {
            switch (argument)
            {
                case "side":
                    switch (value)
                    {
                        case "client":
                            isIntegratedServer = false;
                            side = "client";
                            break;
                        case "server":
                            isIntegratedServer = false;
                            side = "server";
                            break;
                        case "both":
                            isIntegratedServer = true;
                            side = "both";
                            break;
                    }

                    break;
                case "username":
                    playerName = value;
                    break;
                case "password":
                    playerPassword = value;
                    break;
            }
        }
    }
}
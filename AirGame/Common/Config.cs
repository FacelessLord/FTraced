namespace GlLib.Common
{
    public class Config
    {
        public static bool isIntegratedServer = true;
        public static string side = "both";
        public static string playerName = "Noname";
        public static string playerPassword = "";

        public static void ProcessArgument(string _argument, string _value)
        {
            switch (_argument)
            {
                case "side":
                    switch (_value)
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
                    playerName = _value;
                    break;
                case "password":
                    playerPassword = _value;
                    break;
            }
        }
    }
}
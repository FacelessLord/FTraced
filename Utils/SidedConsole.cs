using System;
using GlLib.Common;

namespace GlLib.Utils
{
    public static class SidedConsole
    {
        public static string GetSidePrefix()
        {
            return "[" + Proxy.GetSide() + "]";
        }

        public static void Write(object text)
        {
            Console.Write(GetSidePrefix() + text);
        }

        public static void WriteLine(object text)
        {
            Console.WriteLine(GetSidePrefix() + text);
        }
    }
}
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

        public static void Write(object _text)
        {
            Console.Write(GetSidePrefix() + _text);
        }

        public static void WriteLine(object _text)
        {
            Console.WriteLine(GetSidePrefix() + _text);
        }
        
        public static void WriteErrorLine(object _text)
        {
            Console.Error.WriteLine(GetSidePrefix() + _text);
        }
    }
}
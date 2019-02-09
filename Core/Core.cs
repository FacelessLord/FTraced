using System;
using GlLib.Graphic;
using GlLib.Map;
using GlLib.Registries;
using GlLib.Utils;
using OpenTK;

namespace GlLib.Core
{
    internal static class Core
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Blocks.Register();
            Registries.Entities.Register();
            GraphicCore.Run();
        }

        public static World World = new World("maps/testmap1.json");

    }
}
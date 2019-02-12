using System;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common.Map;
using GlLib.Common.Registries;

namespace GlLib.Common
{
    internal static class Core
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Hello World! {1/16}");
            Blocks.Register();
            KeyBinds.Register();
            Registries.Entities.Register();
            World.LoadWorld();
            GraphicCore.Run();
        }

        public static World World = new World("maps/testmap1.json");
    }
}
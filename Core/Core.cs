using System;
using GlLib.Graphic;
using GlLib.Map;
using OpenTK;

namespace GlLib.Core
{
    internal static class Core
    {
        public static void Main(string[] args)
        {
            Random rand = new Random();
            var blockRegistry = new[] {autumnGrassStone, autumnGrass, grass};
            for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
                blocks[i, j] = blockRegistry[rand.Next(3)];
            Console.WriteLine("Hello World!");
            GraphicCore.Run();
        }

        public static TerrainBlock grass = new GrassBlock();
        public static TerrainBlock autumnGrass = new AutumnGrassBlock();
        public static TerrainBlock autumnGrassStone = new AutumnGrassWithStone();
        public static TerrainBlock path = new Path();
        public static TerrainBlock[,] blocks = new TerrainBlock[16, 16];

    }
}
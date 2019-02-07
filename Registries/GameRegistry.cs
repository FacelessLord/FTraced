using System;
using System.Collections;
using System.Linq.Expressions;
using System.Net.Json;

namespace GlLib.Map
{
    public class GameRegistry
    {
        public static Hashtable blocks = new Hashtable();

        public static void RegisterBlock(TerrainBlock block)
        {
            try
            {
                blocks.Add(block.GetName(),block);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Block with name {block.GetName()} had already been registerded");
                throw;
            }
        }
        
        public static TerrainBlock GetBlockFromName(string blockName)
        {
            if (blocks.ContainsKey(blockName))
                return (TerrainBlock) blocks[blockName];
            return null;
        }
    }
}
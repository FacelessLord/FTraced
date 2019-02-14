using System;
using System.Collections;
using GlLib.Common.Entities;

namespace GlLib.Common.Map
{
    public class GameRegistry
    {
        public static Hashtable blocks = new Hashtable();
        public static Hashtable blocksById = new Hashtable();
        public static Hashtable entities = new Hashtable();

        public static void RegisterBlock(TerrainBlock block)
        {
            try
            {
                int id = blocks.Count;
                blocks.Add(block.GetName(),block);
                blocksById.Add(id,block);
                block._id = id;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Block with name {block.GetName()} had already been registerded");
                throw;
            }
        }

        public static void RegisterEntity(string name, Type entityType)
        {
            try
            {
                Console.WriteLine("Register: " + name);
                entities.Add(name, entityType);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Entity with name {name} had already been registered");
                throw;
            }
        }

        public static TerrainBlock GetBlockFromName(string blockName)
        {
            if (blocks.ContainsKey(blockName))
                return (TerrainBlock) blocks[blockName];
            return null;
        }
        
        public static TerrainBlock GetBlockFromId(int id)
        {
            return (TerrainBlock)blocksById[id];
        }
        
        public static Entity GetEntityFromName(string entityName,params object[] args)
        {
            if (entities.ContainsKey(entityName))
            {
                Type clazz =(Type) entities[entityName];
                return (Entity) Activator.CreateInstance(clazz,args);
            }

            return null;
        }
    }
}
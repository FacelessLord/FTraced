using System;
using System.Collections;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Common.Map
{
    public class GameRegistry
    {
        public static Hashtable _blocks = new Hashtable();
        public static Hashtable _blocksById = new Hashtable();
        public static Hashtable _entities = new Hashtable();

        public static void RegisterBlock(TerrainBlock block)
        {
            try
            {
                int id = _blocks.Count;
                _blocks.Add(block.GetName(),block);
                _blocksById.Add(id,block);
                block._id = id;
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine($"Block with name {block.GetName()} had already been registerded");
                throw;
            }
        }

        public static void RegisterEntity(string name, Type entityType)
        {
            try
            {
                SidedConsole.WriteLine("Register: " + name);
                _entities.Add(name, entityType);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine($"Entity with name {name} had already been registered");
                throw;
            }
        }

        public static TerrainBlock GetBlockFromName(string blockName)
        {
            if (_blocks.ContainsKey(blockName))
                return (TerrainBlock) _blocks[blockName];
            return null;
        }
        
        public static TerrainBlock GetBlockFromId(int id)
        {
            return (TerrainBlock)_blocksById[id];
        }
        
        public static Entity GetEntityFromName(string entityName,params object[] args)
        {
            if (_entities.ContainsKey(entityName))
            {
                Type clazz =(Type) _entities[entityName];
                return (Entity) Activator.CreateInstance(clazz,args);
            }

            return null;
        }
    }
}
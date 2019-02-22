using System;
using System.Collections;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Common.Map
{
    public class GameRegistry
    {
        public Hashtable _blocks = new Hashtable();
        public Hashtable _blocksById = new Hashtable();
        public Hashtable _entities = new Hashtable();

        public void RegisterBlock(TerrainBlock block)
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

        public void RegisterEntity(string name, Type entityType)
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

        public TerrainBlock GetBlockFromName(string blockName)
        {
            if (_blocks.ContainsKey(blockName))
                return (TerrainBlock) _blocks[blockName];
            return null;
        }
        
        public TerrainBlock GetBlockFromId(int id)
        {
            return (TerrainBlock)_blocksById[id];
        }
        
        public Entity GetEntityFromName(string entityName,params object[] args)
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
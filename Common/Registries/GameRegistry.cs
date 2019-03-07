using System;
using System.Collections;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Common.Map
{
    public class GameRegistry
    {
        public Hashtable blocks = new Hashtable();
        public Hashtable blocksById = new Hashtable();
        public Hashtable entities = new Hashtable();

        public void RegisterBlock(TerrainBlock block)
        {
            try
            {
                var id = blocks.Count;
                blocks.Add(block.GetName(), block);
                blocksById.Add(id, block);
                block.id = id;
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
                entities.Add(name, entityType);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine($"Entity with name {name} had already been registered");
                throw;
            }
        }

        public TerrainBlock GetBlockFromName(string blockName)
        {
            if (blocks.ContainsKey(blockName))
                return (TerrainBlock) blocks[blockName];
            return null;
        }

        public TerrainBlock GetBlockFromId(int id)
        {
            return (TerrainBlock) blocksById[id];
        }

        public Entity GetEntityFromName(string entityName, params object[] args)
        {
            if (entities.ContainsKey(entityName))
            {
                var clazz = (Type) entities[entityName];
                return (Entity) Activator.CreateInstance(clazz, args);
            }

            return null;
        }
    }
}
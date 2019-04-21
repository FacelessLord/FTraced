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

        public void RegisterBlock(TerrainBlock _block)
        {
            try
            {
                var id = blocks.Count;
                blocks.Add(_block.GetName(), _block);
                blocksById.Add(id, _block);
                _block.id = id;
            }
            catch (Exception)
            {
                SidedConsole.WriteLine($"Block with name {_block.GetName()} had already been registerded");
                throw;
            }
        }

        public void RegisterEntity(string _name, Type _entityType)
        {
            try
            {
                SidedConsole.WriteLine("Register: " + _name);
                entities.Add(_name, _entityType);
            }
            catch (Exception)
            {
                SidedConsole.WriteLine($"Entity with name {_name} had already been registered");
                throw;
            }
        }

        public TerrainBlock GetBlockFromName(string _blockName)
        {
            if (blocks.ContainsKey(_blockName))
                return (TerrainBlock) blocks[_blockName];
            return null;
        }

        public TerrainBlock GetBlockFromId(int _id)
        {
            return (TerrainBlock) blocksById[_id];
        }

        public Entity GetEntityFromName(string _entityName, params object[] _args)
        {
            if (entities.ContainsKey(_entityName))
            {
                var clazz = (Type) entities[_entityName];
                return (Entity) Activator.CreateInstance(clazz, _args);
            }

            return null;
        }
    }
}
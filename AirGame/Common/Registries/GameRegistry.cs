using System;
using System.Collections;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Registries
{
    public class GameRegistry
    {
        public Hashtable blocks = new Hashtable();
        public Hashtable blocksById = new Hashtable();
        public Hashtable entities = new Hashtable();
        public Hashtable items = new Hashtable();
        public Hashtable itemsById = new Hashtable();

        public void RegisterItem(Item _item)
        {
            try
            {
                SidedConsole.WriteLine("Registered: " + _item.GetName(new ItemStack(_item)));
                var id = items.Count;
                items.Add(_item.unlocalizedName, _item);
                itemsById.Add(id, _item);
                _item.id = id;
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine($"Item with name {_item.unlocalizedName} had already been registered");
                throw;
            }
        }

        public void RegisterBlock(TerrainBlock _block)
        {
            try
            {
                SidedConsole.WriteLine("Registered: " + _block.GetName());
                var id = blocks.Count;
                blocks.Add(_block.GetName(), _block);
                blocksById.Add(id, _block);
                _block.id = id;
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine($"Block with name {_block.GetName()} had already been registered");
                throw;
            }
        }

        public void RegisterEntity(string _name, Type _entityType)
        {
            try
            {
                SidedConsole.WriteLine("Registered: " + _name);
                entities.Add(_name, _entityType);
            }
            catch (Exception e)
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

        public Item GetItemFromId(int _itemId)
        {
            if (items.ContainsKey(_itemId)) return (Item) items[_itemId];

            return null;
        }
    }
}
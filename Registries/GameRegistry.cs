using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Json;
using System.Reflection;
using GlLib.Entities;
using GlLib.Utils;

namespace GlLib.Map
{
    public class GameRegistry
    {
        public static Hashtable blocks = new Hashtable();
        public static Hashtable entities = new Hashtable();

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
        public static void RegisterEntity(Entity entity)
        {
            try
            {
                Console.WriteLine("Register: "+entity.GetName());
                entities.Add(entity.GetName(),entity.GetType());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Entity with name {entity.GetName()} had already been registered");
                throw;
            }
        }
        
        public static TerrainBlock GetBlockFromName(string blockName)
        {
            if (blocks.ContainsKey(blockName))
                return (TerrainBlock) blocks[blockName];
            return null;
        }
        
        
        public static Entity GetEntityFromName(string entityName,World world,RestrictedVector3D pos)
        {
            Console.WriteLine(entityName);
            foreach (var key in entities.Keys)
            {   
                Console.WriteLine(key);
            }
            if (entities.ContainsKey(entityName))
            {
                Type clazz =(Type) entities[entityName];
                return (Entity) Activator.CreateInstance(clazz,world,pos);
            }

            return null;
        }
    }
}
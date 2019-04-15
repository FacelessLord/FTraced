using System.Net.Json;

namespace GlLib.Common.Map
{
    public static class WorldManager
    {
        public static void LoadWorld(World _world, JsonObjectCollection _worldCollection)
        {
            _world.jsonObj = _worldCollection;
            _world.width = (int) ((JsonNumericValue) _worldCollection[0]).Value;
            _world.height = (int) ((JsonNumericValue) _worldCollection[1]).Value;

            _world.chunks = new Chunk[_world.width, _world.height];

            for (var i = 0; i < _world.width; i++)
            for (var j = 0; j < _world.height; j++)
                _world[i, j] = new Chunk(_world, i, j);
            _world.LoadWorld();
        }

        public static void LoadEntitiesAtChunk(World _world, int _x, int _y, JsonObjectCollection _entityCollection)
        {
            _world[_x,_y].LoadChunkEntities(_entityCollection);
        }
        
        public static void LoadEntities(World _world, JsonObjectCollection _entityCollection)
        {
            //todo
        }
    }
}
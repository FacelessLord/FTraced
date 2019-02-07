using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Json;
using GlLib.Entities;
using GlLib.Events;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Map
{
    public class World
    {
        public Chunk[,] _chunks;

        public string _mapName;

        public Chunk this[int i, int j]
        {
            get
            {
                if (i + _width / 2 < 0 || i + _width / 2 > _width)
                    return null;
                if (j + _height / 2 < 0 || j + _height / 2 > _height)
                    return null;
                return _chunks[i + _width / 2, j + _height / 2];
            }
            set
            {
                if (i + _width / 2 < 0 || i + _width / 2 > _width)
                    return;
                if (j + _height / 2 < 0 || j + _height / 2 > _height)
                    return;
                _chunks[i + _width / 2, j + _height / 2] = value;
            }
        }

        public int _width;
        public int _height;
        public JsonObjectCollection _jsonObj;

        public World(string mapName)
        {
            _jsonObj = LoadJsonWorld(mapName);
            _width = (int) ((JsonNumericValue) _jsonObj[0]).Value;
            _height = (int) ((JsonNumericValue) _jsonObj[1]).Value;
            
            _chunks = new Chunk[_width, _height];

            for (int i = -_width / 2; i < _width - _width / 2; i++)
            {
                for (int j = -_height / 2; j < _height - _height / 2; j++)
                {
                    this[i, j] = new Chunk(this, i, j);
                    this[i,j].LoadChunk(this,i,j);
                }
            }
        }

        public JsonObjectCollection LoadJsonWorld(string name)
        {
            var parser = new JsonTextParser();
            var mapCode = File.ReadAllText(name);
            var obj = parser.Parse(mapCode);
            var mainCollection = (JsonObjectCollection) obj;
            return mainCollection;
        }

        public void SpawnEntity(Entity e)
        {
            if (EventBus.OnEntitySpawn(e)) return;

            e._chunkObj._entities[e._position._z].Add(e);
        }

        public void SetBlockAt(TerrainBlock block, int x, int y)
        {
            int blockX = x % 16;
            int blockY = y % 16;
            int chunkX = x / 16;
            int chunkY = y / 16;

            Chunk chunk = this[chunkX, chunkY];
            chunk[blockX, blockY] = block;
        }

        public List<Entity> GetEntitiesWithinAaBb(AxisAlignedBb aabb)
        {
            List<Entity> entities = new List<Entity>();

            List<Chunk> chunks = new List<Chunk>();

            int chkStartX = aabb.StartXi / 16;
            int chkStartY = aabb.StartYi / 16;
            int chkEndX = aabb.EndXi / 16;
            int chkEndY = aabb.EndYi / 16;

            for (int i = chkStartX; i <= chkEndX; i++)
            for (int j = chkStartY; j <= chkEndY; j++)
            {
                Chunk chk = this[i, j];
                if (chk != null)
                    chunks.Add(chk);
            }

            foreach (var chk in chunks)
            {
                foreach (var height in chk._entities)
                {
                    foreach (var entity in height)
                    {
                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }

        public List<Entity> GetEntitiesWithinAaBbAndHeight(AxisAlignedBb aabb, int height)
        {
            List<Entity> entities = new List<Entity>();

            List<Chunk> chunks = new List<Chunk>();

            int chkStartX = aabb.StartXi / 16;
            int chkStartY = aabb.StartYi / 16;
            int chkEndX = aabb.EndXi / 16;
            int chkEndY = aabb.EndYi / 16;

            for (int i = chkStartX; i <= chkEndX; i++)
            for (int j = chkStartY; j <= chkEndY; j++)
            {
                Chunk chk = this[i, j];
                if (chk != null)
                    chunks.Add(chk);
            }

            foreach (var chk in chunks)
            {
                List<Entity> chkEntities = chk._entities[height];
                foreach (var entity in chkEntities)
                {
                    if (entity.GetAaBb().IntersectsWith(aabb))
                        entities.Add(entity);
                }
            }

            return entities;
        }

        public void Render(int x, int y)
        {
            GL.PushMatrix();
            for (int i = -_width/2; i <= _width/2; i++)
            for (int j = -_height/2; j <= _height/2; j++)
                if (this[i + x, j + y]._isLoaded)
                {
                    this[i + x, j + y].RenderChunk(i, j);
                }

            GL.PopMatrix();
        }
    }
}
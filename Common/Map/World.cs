using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Json;
using GlLib.Common.Entities;
using GlLib.Common.Events;
using GlLib.Client.Input;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Map
{
    public class World
    {
        public Chunk[,] _chunks;

        public int _worldId;

        public List<Player> _players = new List<Player>();

        public Chunk this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= _width)
                    return null;
                if (j < 0 || j >= _height)
                    return null;
                return _chunks[i, j];
            }
            set
            {
                if (i < 0 || i > _width)
                    return;
                if (j < 0 || j > _height)
                    return;
                _chunks[i, j] = value;
            }
        }

        public int _width;
        public int _height;
        public JsonObjectCollection _jsonObj;
        public string _mapName;

        public World(string mapName,int worldId)
        {
            _mapName = mapName;
            _worldId = worldId;
            _jsonObj = LoadWorldJson(mapName);
            _width = (int) ((JsonNumericValue) _jsonObj[0]).Value;
            _height = (int) ((JsonNumericValue) _jsonObj[1]).Value;

            _chunks = new Chunk[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    this[i, j] = new Chunk(this, i, j);
                }
            }
        }
        
        public void LoadWorld()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if(!this[i,j]._isLoaded)
                        this[i, j].LoadChunk(this, i, j);
                }
            }
        }

        public void UnloadWorld()
        {
            List<JsonObject> objects = new List<JsonObject>();
            objects.Add(new JsonNumericValue("Width", _width));
            objects.Add(new JsonNumericValue("Height", _height));
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    objects.Add(this[i, j].UnloadChunk(this, i, j));
                }
            }

            JsonObjectCollection mainColl = new JsonObjectCollection(objects);
            
            FileStream fs = File.OpenWrite(_mapName);
            TextWriter tw = new StreamWriter(fs);
            mainColl.WriteTo(tw);
            tw.Flush();
            fs.Flush();
            tw.Close();
            fs.Close();
        }

        public JsonObjectCollection LoadWorldJson(string name)
        {
            var parser = new JsonTextParser();
            var mapCode = File.ReadAllText("maps/"+name);
            var obj = parser.Parse(mapCode);
            var mainCollection = (JsonObjectCollection) obj;
            return mainCollection;
        }

        public void SpawnEntity(Entity e)
        {
            if (EventBus.OnEntitySpawn(e)) return;

            if (e is Player p)
                _players.Add(p);
            if (e._chunkObj == null)
                e._chunkObj = Entity.GetProjection(e._position, this);
            e._chunkObj._entities[e._position._z].Add(e);
            SidedConsole.WriteLine($"Entity {e} spawned in world");
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
            PlanarVector xAxis = new PlanarVector(Chunk.BlockWidth / 2, Chunk.BlockHeight / 2);
            PlanarVector yAxis = new PlanarVector(Chunk.BlockWidth / 2, -Chunk.BlockHeight / 2);
            GL.PushMatrix();
            GL.Translate(-Math.Max(_width, _height) * Chunk.BlockWidth * 5, 0, 0);
            for (int i = 0; i < _width; i++)
            for (int j = _width - 1; j >= 0; j--)
                if (this[i + x, j + y]._isLoaded)
                {
                    this[i + x, j + y].RenderChunk(i, j, xAxis, yAxis);
                }

            for (int i = 0; i < _width; i++)
            for (int j = _width - 1; j >= 0; j--)
                if (this[i + x, j + y]._isLoaded)
                {

                    foreach (var level in this[i + x, j + y]._entities)
                    {
                        foreach (var entity in level)
                        {
                            PlanarVector coord = xAxis * (entity._position._x - 8) + yAxis * (entity._position._y - 8);
                            GL.PushMatrix();

                            GL.Translate(coord._x, coord._y, 0);
                            entity.Render(xAxis, yAxis);
                            GL.PopMatrix();
                        }
                    }
                }

            GL.PopMatrix();
        }

        public void Update()
        {
            foreach (var pair in _entityRemoveQueue)
            {
                pair.chk._entities[pair.e._position._z].Remove((pair.e));
            }

            _entityRemoveQueue.Clear();
            foreach (var pair in _entityAddQueue)
            {
                pair.chk._entities[pair.e._position._z].Add((pair.e));
            }

            _entityAddQueue.Clear();
            foreach (var player in _players)
            {
                player._acceleration = new PlanarVector();
                KeyBinds.Update(player);
                
//                SidedConsole.WriteLine(player._nickname);
                player.Update();
            }

            foreach (var chunk in _chunks)
            {
                if (chunk._isLoaded)
                {
                    chunk.Update();
                }
            }
        }

        public List<(Entity e, Chunk chk)> _entityRemoveQueue = new List<(Entity e, Chunk chk)>();
        public List<(Entity e, Chunk chk)> _entityAddQueue = new List<(Entity e, Chunk chk)>();

        public void ChangeEntityChunk(Entity e, Chunk next)
        {
            _entityRemoveQueue.Add((e, e._chunkObj));
            _entityAddQueue.Add((e, next));
            e._chunkObj = next;
        }
    }
}
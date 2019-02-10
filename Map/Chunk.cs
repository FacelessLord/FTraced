using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Graphic;
using DiggerLib;
using GlLib.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Map
{
    public class Chunk
    {
        public const int Heights = 8;

        public World _world;
        public TerrainBlock[,] _blocks; // = new TerrainBlock[16,16];
    
        public List<Entity>[] _entities = new List<Entity>[Heights];

        public int _chunkX;
        public int _chunkY;

        public TerrainBlock this[int i, int j]
        {
            get => _blocks[i, j];
            set => _blocks[i, j] = value;
        }

        public Chunk(World world, int x, int y)
        {
            _world = world;
            _chunkX = x;
            _chunkY = y;
            _blocks = new TerrainBlock[16, 16];
            for (int i = 0; i < Heights; i++)
            {
                _entities[i] = new List<Entity>();
            }
        }

        public const double BlockWidth = 64;
        public const double BlockHeight = 32;

        public void RenderChunk(double centerX, double centerY)
        {
            GL.PushMatrix();

            GL.Translate((centerX + centerY) * BlockWidth * 8, (centerX - centerY) * BlockHeight * 8, 0);

            PlanarVector xAxis = new PlanarVector(BlockWidth / 2, BlockHeight / 2);
            PlanarVector yAxis = new PlanarVector(BlockWidth / 2, -BlockHeight / 2);

            //GL.Color3(0.75,0.75,0.75);
            for (int i = 7; i > -9; i--)
            {
                for (int j = -8; j < 8; j++)
                {
                    TerrainBlock block = _blocks[i + 8, j + 8];
                    if (block == null) continue;
                    if (!block.RequiresSpecialRenderer(_world, i + 8, j + 8))
                    {
                        Texture btexture = block.GetTexture(_world, i + 8, j + 8);
                        Vertexer.BindTexture(btexture);
                        PlanarVector coord = xAxis * i + yAxis * j;
                        GL.PushMatrix();

                        GL.Translate(coord._x, coord._y, 0);
                        //Vertexer.DrawTexturedModalRect(btexture,0, 0, 0, 0, btexture.width, btexture.height);

                        Vertexer.StartDrawingQuads();

                        Vertexer.VertexWithUvAt(BlockWidth, 0, 1, 0);
                        Vertexer.VertexWithUvAt(BlockWidth, BlockHeight, 1, 1);
                        Vertexer.VertexWithUvAt(0, BlockHeight, 0, 1);
                        Vertexer.VertexWithUvAt(0, 0, 0, 0);

                        Vertexer.Draw();

                        GL.PopMatrix();
                    }
                    else
                    {
                        block.GetSpecialRenderer(_world, i, j).Render(_world, i, j);
                    }
                }
            }

            GL.PopMatrix();

            foreach (var level in _entities)
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

        public bool _isLoaded = false;

        public void LoadChunk(World world, int x, int y)
        {
            JsonObjectCollection mainCollection = world._jsonObj;
            JsonObjectCollection chunkCollection = null;

            foreach (var obj in mainCollection)
            {
                if (obj is JsonObjectCollection chk)
                {
                    if (chk.Name == x + "," + y)
                    {
                        chunkCollection = chk;
                        break;
                    }
                }
            }

            if (chunkCollection != null)
            {
                _blocks = new TerrainBlock[16, 16];
                foreach (var entry in chunkCollection)
                {
                    if (entry is JsonStringValue blockName)
                    {
                        if (blockName.Value.StartsWith("block."))
                        {
                            string[] coords = blockName.Name.Split(',');
                            int i = int.Parse(coords[0]);
                            int j = int.Parse(coords[1]);

//                        Console.WriteLine($"Chunk's block {i}x{j} is loaded");
                            _blocks[i, j] = GameRegistry.GetBlockFromName(blockName.Value);
                        }
                    }

                    if (entry is JsonNumericValue num)
                    {
                        string[] coords = num.Name.Split(',');
                        int i = int.Parse(coords[0]);
                        int j = int.Parse(coords[1]);
                        
                        _blocks[i, j] = GameRegistry.GetBlockFromId((int)num.Value);
                    }

                    if (entry is JsonObjectCollection collection)
                    {
                        if (collection.Name.StartsWith("Rect"))
                        {
                            JsonObject preBorders = collection[0];
                            if (preBorders is JsonArrayCollection borders)
                            {
                                JsonObject preBlock = collection[1];
                                if (preBlock is JsonStringValue rectBlockName)
                                {
                                    TerrainBlock block = GameRegistry.GetBlockFromName(rectBlockName.Value);
                                    int startX = (int) ((JsonNumericValue) borders[0]).Value;
                                    int startY = (int) ((JsonNumericValue) borders[1]).Value;
                                    int endX = (int) ((JsonNumericValue) borders[2]).Value;
                                    int endY = (int) ((JsonNumericValue) borders[3]).Value;
                                    for (int i = startX; i <= endX; i++)
                                    {
                                        for (int j = startY; j <= endY; j++)
                                        {
                                            _blocks[i, j] = block;
                                        }
                                    }
                                }
                            }
                        }
                        else //Entity
                        {
                            Entity entity = Entity.LoadFromJson(collection, _world, this);
                            _world.SpawnEntity(entity);
                        }
                    }
                }

                Console.WriteLine($"Chunk {_chunkX}x{_chunkY} is loaded");
                _isLoaded = true;
            }
        }

        public JsonObjectCollection UnloadChunk(World world, int x, int y)
        {
            List<JsonObject> objects = new List<JsonObject>();
            foreach (var height in _entities)
            {
                foreach (var entity in height)
                {
                    objects.Add(entity.CreateJsonObj());
                }
            }

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    TerrainBlock block = this[i,j];
                    if(block != null)
                        objects.Add(new JsonNumericValue($"{i},{j}",block.id));
                }
            }
            return new JsonObjectCollection($"{x},{y}",objects);
        }

        public void Update()
        {
            foreach (var level in _entities)
            {
                foreach (var entity in level)
                {
                    entity.Update();
                }
            }
        }
    }
}
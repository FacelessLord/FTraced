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
                for (int j = 7; j > -9; j--)
                {
                    TerrainBlock block = _blocks[i + 8, j + 8];
                    if (block == null) continue;
                    Texture btexture = block.GetTexture(i + 8, j + 8);
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
            }

            foreach (var level in _entities)
            {
                foreach (var entity in level)
                {
                    PlanarVector coord = xAxis * (entity._position._x % 16 - 8) + yAxis * (entity._position._y % 16 - 8);
                    GL.PushMatrix();

                    GL.Translate(coord._x, coord._y, 0);
                    entity.Render(xAxis, yAxis);
                    GL.PopMatrix();
                }
            }

            GL.PopMatrix();
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
                            double posX = 0;
                            double posY = 0;
                            int z = 0;
                            double velX = 0;
                            double velY = 0;
                            string id = "null";
                            string rawTag = "";

                            foreach (var entityField in collection)
                            {
                                if (entityField is JsonArrayCollection arr)
                                {
                                    switch (arr.Name)
                                    {
                                        case "pos":
                                            (posX, posY, z) = (((JsonNumericValue) arr[0]).Value,
                                                ((JsonNumericValue) arr[1]).Value,
                                                (int) ((JsonNumericValue) arr[2]).Value);
                                            break;
                                        case "vel":
                                            (velX, velY) = (((JsonNumericValue) arr[0]).Value,
                                                ((JsonNumericValue) arr[1]).Value);
                                            break;
                                    }
                                }

                                if (entityField is JsonStringValue str)
                                {
                                    switch (str.Name)
                                    {
                                        case "id":
                                            id = str.Value;
                                            break;
                                        case "tag":
                                            rawTag = str.Value;
                                            break;
                                    }
                                }
                            }

                            Entity entity =
                                GameRegistry.GetEntityFromName(id, _world, new RestrictedVector3D(posX+x*16, posY+y*16, z));
                            entity._velocity = new PlanarVector(velX, velY);
                            entity._chunkObj = this;
                            entity._nbtTag = NbtTag.FromString(rawTag);
                            entity.LoadFromNbt(entity._nbtTag);
                            _world.SpawnEntity(entity);
                            Console.WriteLine($"Entity with id \"{id}\" has been loaded");
                        }
                    }
                }

                Console.WriteLine($"Chunk {_chunkX}x{_chunkY} is loaded");
                _isLoaded = true;
            }
        }

        public void UnloadChunk(World world, int x, int y)
        {
            //todo saving by creating json with blocks and entities(using ReadFromNBT)
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
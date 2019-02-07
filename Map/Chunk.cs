using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Graphic;
using DiggerLib;
using GlLib.Entities;
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

        public Chunk(World world,int x, int y, TerrainBlock[,] blocks)
        {
            _world = world;
            _chunkX = x;
            _chunkY = y;
            _blocks = blocks;
            _isLoaded = true;
        }
        
        public Chunk(World world,int x, int y)
        {
            _world = world;
            _chunkX = x;
            _chunkY = y;
            _blocks = new TerrainBlock[16,16];
        }

        public const double BlockWidth = 64;
        public const double BlockHeight = 32;

        public void RenderChunk(double centerX, double centerY)
        {
            GL.PushMatrix();

            GL.Translate((centerX+centerY)*BlockWidth*8, (centerX-centerY)*BlockHeight*8, 0);

            Vector xAxis = new Vector(BlockWidth / 2, BlockHeight / 2);
            Vector yAxis = new Vector(BlockWidth / 2, -BlockHeight / 2);

            //GL.Color3(0.75,0.75,0.75);
            for (int i = 7; i > -9; i--)
            {
                for (int j = 7; j > -9; j--)
                {
                    TerrainBlock block = _blocks[i+8, j+8];
                    if(block == null) continue;
                    Texture btexture = block.GetTexture(i+8, j+8);
                    Vertexer.BindTexture(btexture);
                    Vector coord = xAxis * i + yAxis * j;
                    GL.PushMatrix();

                    GL.Translate(coord.X, coord.Y, 0);
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
                _blocks = new TerrainBlock[16, 16];//todo
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        foreach (var block in chunkCollection)
                        {
                            if (block.Name == i + "," + j && block is JsonStringValue blockName)
                            {
                                Console.WriteLine($"Chunk's block {i}x{j} is loaded");
                                _blocks[i, j] = GameRegistry.GetBlockFromName(blockName.Value);
                                break;
                            }
                        }
                    }
                }

                foreach (var entry in chunkCollection)
                {
                    if (entry is JsonObjectCollection chunk && chunk.Name.StartsWith("Rect"))
                    {
                        JsonObject preBorders = chunk[0];
                        if (preBorders is JsonArrayCollection borders)
                        {
                            JsonObject preBlock = chunk[1];
                            if (preBlock is JsonStringValue blockName)
                            {
                                TerrainBlock block = GameRegistry.GetBlockFromName(blockName.Value);
                                int startX = (int)((JsonNumericValue)borders[0]).Value;
                                int startY = (int)((JsonNumericValue)borders[1]).Value;
                                int endX = (int)((JsonNumericValue)borders[2]).Value;
                                int endY = (int)((JsonNumericValue)borders[3]).Value;
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
                }
                
                Console.WriteLine($"Chunk {_chunkX}x{_chunkY} is loaded");
                _isLoaded = true;
            }
        }
        public void UnloadChunk(World world, int x, int y)
        {
            
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Client.Graphic;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Map
{
    public class Chunk
    {
        public const int Heights = 8;

        public const double BlockWidth = 64;
        public const double BlockHeight = 32;
        public TerrainBlock[,] blocks; // = new TerrainBlock[16,16];

        public int chunkX;
        public int chunkY;

        public List<Entity>[] entities = new List<Entity>[Heights];

        public bool isLoaded;

        public World world;

        public Chunk(World _world, int _x, int _y)
        {
            world = _world;
            chunkX = _x;
            chunkY = _y;
            blocks = new TerrainBlock[16, 16];
            for (var i = 0; i < Heights; i++) entities[i] = new List<Entity>();
        }

        public TerrainBlock this[int _i, int _j]
        {
            get => blocks[_i, _j];
            set => blocks[_i, _j] = value;
        }

        public void RenderChunk(double _centerX, double _centerY, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();

            GL.Translate(_centerX * BlockWidth * 16, _centerY * BlockHeight * 16, 0);

            //GL.Color3(0.75,0.75,0.75);
            for (var i = 7; i > -9; i--)
            for (var j = -8; j < 8; j++)
            {
                var block = blocks[i + 8, j + 8];
                if (block == null) continue;
                if (!block.RequiresSpecialRenderer(world, i + 8, j + 8))
                {
                    var btexture = Vertexer.LoadTexture(block.TextureName);
                    Vertexer.BindTexture(btexture);
                    var coord = _xAxis * (i + 8) + _yAxis * (j + 8);
                    GL.PushMatrix();

                    GL.Translate(coord.x, coord.y, 0);
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
                    block.GetSpecialRenderer(world, i, j).Render(world, i, j);
                }
            }

            GL.PopMatrix();
        }

        public void LoadFromJson(JsonObjectCollection _chunkCollection, bool loadEntities)
        {
            var stashedBlocks = new Hashtable();
            if (world.FromStash)
                stashedBlocks = Stash.GetBlocks();
            if (_chunkCollection != null)
            {
                blocks = new TerrainBlock[16, 16];
                foreach (var entry in _chunkCollection)
                    switch (entry)
                    {
                        case JsonStringValue gameObject when gameObject.Value.StartsWith("block."):
                        {
                            var coords = gameObject.Name.Split(',');
                            var i = int.Parse(coords[0]);
                            var j = int.Parse(coords[1]);

//                        Console.WriteLine($"Chunk's block {i}x{j} is loaded");
                            if (world.FromStash)
                                blocks[i, j] = (TerrainBlock) stashedBlocks[gameObject.Value];

                            else
                                blocks[i, j] = Proxy.GetRegistry().GetBlockFromName(gameObject.Value);

                            break;
                        }
                        case JsonNumericValue num:
                        {
                            var coords = num.Name.Split(',');
                            var i = int.Parse(coords[0]);
                            var j = int.Parse(coords[1]);

                            if (world.FromStash)
                                blocks[i, j] = (TerrainBlock) stashedBlocks[(int) num.Value];
                            else
                                blocks[i, j] = Proxy.GetRegistry().GetBlockFromId((int) num.Value);

                            break;
                        }

                        case JsonObjectCollection collection:
                        {
                            if (collection.Name.StartsWith("Rect"))
                            {
                                if (world.FromStash)
                                    break;

                                var preBorders = collection[0];
                                if (preBorders is JsonArrayCollection borders)
                                {
                                    var preBlock = collection[1];
                                    if (preBlock is JsonStringValue rectBlockName)
                                    {
                                        var block = Proxy.GetRegistry().GetBlockFromName(rectBlockName.Value);
                                        var startX = (int) ((JsonNumericValue) borders[0]).Value;
                                        var startY = (int) ((JsonNumericValue) borders[1]).Value;
                                        var endX = (int) ((JsonNumericValue) borders[2]).Value;
                                        var endY = (int) ((JsonNumericValue) borders[3]).Value;
                                        for (var i = startX; i <= endX; i++)
                                        for (var j = startY; j <= endY; j++)
                                            blocks[i, j] = block;
                                    }
                                }
                            }

                            if (collection.Name.StartsWith("entity") && loadEntities)
                            {
                                var entity = Proxy.GetRegistry().GetEntityFromJson(collection);
                                world.SpawnEntity(entity);
                            }

                            break;
                        }
                    }

                SidedConsole.WriteLine($"Chunk {chunkX}x{chunkY} is loaded");
                isLoaded = true;
            }
        }

        public List<JsonObject> SaveChunkEntities()
        {
            var objects = new List<JsonObject>();
            foreach (var height in entities)
            foreach (var entity in height)
                objects.Add(entity.CreateJsonObject());

            return objects;
        }

        public void Update()
        {
            foreach (var level in entities)
            foreach (var entity in level)
                entity.Update();
        }
    }
}
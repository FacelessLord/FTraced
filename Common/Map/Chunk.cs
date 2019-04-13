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

        public Chunk(World world, int x, int y)
        {
            this.world = world;
            chunkX = x;
            chunkY = y;
            blocks = new TerrainBlock[16, 16];
            for (var i = 0; i < Heights; i++) entities[i] = new List<Entity>();
        }

        public TerrainBlock this[int i, int j]
        {
            get => blocks[i, j];
            set => blocks[i, j] = value;
        }

        public void RenderChunk(double centerX, double centerY, PlanarVector xAxis, PlanarVector yAxis)
        {
            GL.PushMatrix();

            GL.Translate((centerX + centerY) * BlockWidth * 8, (centerX - centerY) * BlockHeight * 8, 0);

            //GL.Color3(0.75,0.75,0.75);
            for (var i = 7; i > -9; i--)
            for (var j = -8; j < 8; j++)
            {
                var block = blocks[i + 8, j + 8];
                if (block == null) continue;
                if (!block.RequiresSpecialRenderer((ClientWorld) world, i + 8, j + 8))
                {
                    var btexture = Vertexer.LoadTexture(block.GetTextureName((ClientWorld) world, i + 8, j + 8));
                    Vertexer.BindTexture(btexture);
                    var coord = xAxis * i + yAxis * j;
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
                    block.GetSpecialRenderer((ClientWorld) world, i, j).Render((ClientWorld) world, i, j);
                }
            }

            GL.PopMatrix();

            foreach (var level in entities)
            foreach (var entity in level)
            {
                var coord = xAxis * (entity.Position.x - 8) + yAxis * (entity.Position.y - 8);
                GL.PushMatrix();

                GL.Translate(coord.x, coord.y, 0);
                entity.Render(xAxis, yAxis);
                GL.PopMatrix();
            }
        }

        public void LoadChunk()
        {
            var mainCollection = world.jsonObj;
            JsonObjectCollection chunkCollection = null;

            foreach (var obj in mainCollection)
                if (obj is JsonObjectCollection chk)
                    if (chk.Name == chunkX + "," + chunkY)
                    {
                        chunkCollection = chk;
                        break;
                    }

            if (chunkCollection != null)
            {
                blocks = new TerrainBlock[16, 16];
                foreach (var entry in chunkCollection)
                {
                    if (entry is JsonStringValue gameObject)
                    {
                        if (gameObject.Value.StartsWith("block."))
                        {
                            var coords = gameObject.Name.Split(',');
                            var i = int.Parse(coords[0]);
                            var j = int.Parse(coords[1]);

//                        Console.WriteLine($"Chunk's block {i}x{j} is loaded");
                            blocks[i, j] = Proxy.GetSideRegistry().GetBlockFromName(gameObject.Value);
                        }
                        else //Entity
                        {
                            var entity = Entity.LoadFromJson(gameObject, this);
                            world.SpawnEntity(entity);
                        }
                    }

                    if (entry is JsonNumericValue num)
                    {
                        var coords = num.Name.Split(',');
                        var i = int.Parse(coords[0]);
                        var j = int.Parse(coords[1]);

                        blocks[i, j] = Proxy.GetSideRegistry().GetBlockFromId((int) num.Value);
                    }

                    if (entry is JsonObjectCollection collection)
                        if (collection.Name.StartsWith("Rect"))
                        {
                            var preBorders = collection[0];
                            if (preBorders is JsonArrayCollection borders)
                            {
                                var preBlock = collection[1];
                                if (preBlock is JsonStringValue rectBlockName)
                                {
                                    var block = Proxy.GetSideRegistry().GetBlockFromName(rectBlockName.Value);
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
                }

                SidedConsole.WriteLine($"Chunk {chunkX}x{chunkY} is loaded");
                isLoaded = true;
            }
        }

        public JsonObjectCollection SaveChunkEntities()
        {
            var objects = new List<JsonObject>();
            foreach (var height in entities)
            foreach (var entity in height)
                objects.Add(entity.CreateJsonObj());

            return new JsonObjectCollection($"{chunkX},{chunkY}", objects);
        }

        public void LoadChunkEntities(JsonObjectCollection entityCollection)
        {
            foreach (var entityJson in entityCollection)
            {
                if (entityJson != null)
                {
                    world.SpawnEntity(Entity.LoadFromJson(entityJson as JsonStringValue, this));
                }
            }
        }

        public void Update()
        {
            foreach (var level in entities)
            foreach (var entity in level)
                entity.Update();
        }
    }
}
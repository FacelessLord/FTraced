using System.Collections.Generic;
using GlLib.Graphic;
using DiggerLib;
using GlLib.Entities;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Map
{
    public class Chunk
    {
        public const int Heights = 8;
        
        public TerrainBlock[,] _blocks; // = new TerrainBlock[16,16];

        public List<Entity>[] _entities = new List<Entity>[Heights];
        
        public int _chunkX;
        public int _chunkY;

        public TerrainBlock this[int i, int j]
        {
            get => _blocks[i, j];
            set => _blocks[i, j] = value;
        }

        public Chunk(int x, int y, TerrainBlock[,] blocks)
        {
            _chunkX = x;
            _chunkY = y;
            _blocks = blocks;
        }

        public double _blockWidth = 64;
        public double _blockHeight = 32;

        public void RenderChunk(double centerX, double centerY)
        {
            GL.PushMatrix();

            GL.Translate((centerX+centerY)*_blockWidth*8, (centerX-centerY)*_blockHeight*8, 0);

            Vector xAxis = new Vector(_blockWidth / 2, _blockHeight / 2);
            Vector yAxis = new Vector(_blockWidth / 2, -_blockHeight / 2);

            //GL.Color3(0.75,0.75,0.75);
            for (int i = 7; i > -9; i--)
            {
                for (int j = 7; j > -9; j--)
                {
                    TerrainBlock block = _blocks[i+8, j+8];
                    Texture btexture = block.GetTexture(i+8, j+8);
                    Vertexer.BindTexture(btexture);
                    Vector coord = xAxis * i + yAxis * j;
                    GL.PushMatrix();

                    GL.Translate(coord.X, coord.Y, 0);
                    //Vertexer.DrawTexturedModalRect(btexture,0, 0, 0, 0, btexture.width, btexture.height);

                    Vertexer.StartDrawingQuads();

                    Vertexer.VertexWithUvAt(_blockWidth, 0, 1, 0);
                    Vertexer.VertexWithUvAt(_blockWidth, _blockHeight, 1, 1);
                    Vertexer.VertexWithUvAt(0, _blockHeight, 0, 1);
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
            
        }
        public void UnloadChunk(World world, int x, int y)
        {
            
        }
        
    }
}
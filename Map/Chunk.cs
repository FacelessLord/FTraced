using GlLib.Graphic;
using DiggerLib;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Map
{
    public class Chunk
    {
        public TerrainBlock[,] _blocks; // = new TerrainBlock[16,16];

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

        public double blockWidth = 64;
        public double blockHeight = 32;

        public void RenderChunk(double centerX, double centerY)
        {
            GL.PushMatrix();

            GL.Translate((centerX+centerY)*blockWidth*8, (centerX-centerY)*blockHeight*8, 0);

            Vector xAxis = new Vector(blockWidth / 2, blockHeight / 2);
            Vector yAxis = new Vector(blockWidth / 2, -blockHeight / 2);

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

                    Vertexer.VertexWithUvAt(blockWidth, 0, 1, 0);
                    Vertexer.VertexWithUvAt(blockWidth, blockHeight, 1, 1);
                    Vertexer.VertexWithUvAt(0, blockHeight, 0, 1);
                    Vertexer.VertexWithUvAt(0, 0, 0, 0);

                    Vertexer.Draw();
                    
                    GL.PopMatrix();
                }
            }

            GL.PopMatrix();
        }
    }
}
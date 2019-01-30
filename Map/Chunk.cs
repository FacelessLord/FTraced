using DiggerLib;
using GlLib.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Map
{
    public class Chunk
    {
        public TerrainBlock[,] _blocks = new TerrainBlock[16,16];

        public int _chunkX;
        public int _chunkY;

        public TerrainBlock this[int i,int j]
        {
            get => _blocks[i, j];
            set => _blocks[i, j] = value;
        }
        
        public Chunk(int x,int y,TerrainBlock[,] blocks)
        {
            _chunkX = x;
            _chunkY = y;
            _blocks = blocks;
        }
        
        public void RenderChunk(double centerX, int centerY)
        {
            GL.PushMatrix();
            
            GL.Translate(centerX,centerY,0);
            
            Vector xAxis = new Vector(32,16);
            Vector yAxis = new Vector(32,-16);

            for (int i = -8; i < 8; i++)
            {
                for (int j = -8; j < 8;j++)
                {
                    Vector coord = xAxis * i + yAxis * j;
                    GL.PushMatrix();
                    
                    GL.Translate(coord.X,coord.Y,0);
                    TerrainBlock block = _blocks[i+8, j+8];
                    Texture btexture = block.GetTexture(i,j);
                    Vertexer.BindTexture(btexture);
                    Vertexer.DrawTexturedModalRect(0,0,0,0,btexture.width,btexture.height);
                    
                    GL.PopMatrix();
                }
            }
            
            GL.PopMatrix();
        }
    }
}
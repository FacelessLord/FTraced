using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API
{
    public class Sprite
    {
        public Texture texture;
        public int startU;
        public int startV;
        public int endU;
        public int endV;
        public int countX;
        public int countY;

        /// <summary>
        /// Layouts part of Texture to be rendered as animation or different state of game object
        /// </summary>
        /// <param name="_texture"></param>Texture to layout
        /// <param name="_startU"></param>texture x-coordinate to start from
        /// <param name="_startV"></param>texture y-coordinate to start from
        /// <param name="_endU"></param>texture x-coordinate to go to
        /// <param name="_endV"></param>texture y-coordinate to go to
        /// <param name="_countX"></param>count of frames in sprite in x direction
        /// <param name="_countY"></param>count of frames in sprite in y direction
        public Sprite(Texture _texture, int _startU, int _startV, int _endU, int _endV, int _countX, int _countY)
        {
            texture = _texture;
            (startU, startV) = (_startU, _startV);
            (endU, endV) = (_endU, _endV);
            countX = _countX;
            countY = _countY;
        }

        public void Render(int _stepCount)
        {
            int width = texture.width;
            int height = texture.height;

            int xCount = _stepCount % countX;
            int yCount = _stepCount / countX;
            
            int stepX = (endU - startU) / countX;
            float frameStartX = (startU + stepX * xCount) / (float) texture.width;
            float frameEndX = (startU + stepX * (xCount + 1))/ (float) texture.width;

            int stepY = (endV - startV) / countY;
            float frameStartY = (startV + stepY * yCount) / (float) texture.height;
            float frameEndY = (startV + stepY * (yCount + 1))/ (float) texture.height;

            GL.PushMatrix();
            Vertexer.BindTexture(texture);
            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(0, 0, frameStartX, frameStartY);
            Vertexer.VertexWithUvAt(width, 0, frameEndX, frameStartY);
            Vertexer.VertexWithUvAt(width, height, frameEndX, frameEndY);
            Vertexer.VertexWithUvAt(0, height, frameStartX, frameEndY);

            Vertexer.Draw();
            GL.PopMatrix();
        }
    }
}
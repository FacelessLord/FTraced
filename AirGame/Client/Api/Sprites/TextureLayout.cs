using GlLib.Client.API;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class TextureLayout
    {
        public Layout layout;
        public Texture texture;
//        public 

        /// <summary>
        ///     Layouts part of Texture to be rendered as animation or different state of game object
        /// </summary>
        /// <param name="_texture"></param>
        /// Texture to layout
        /// <param name="_startU"></param>
        /// texture x-coordinate to start from
        /// <param name="_startV"></param>
        /// texture y-coordinate to start from
        /// <param name="_endU"></param>
        /// texture x-coordinate to go to
        /// <param name="_endV"></param>
        /// texture y-coordinate to go to
        /// <param name="_countX"></param>
        /// count of frames in sprite in x direction
        /// <param name="_countY"></param>
        /// count of frames in sprite in y direction
        public TextureLayout(Texture _texture, int _startU, int _startV, int _endU, int _endV, int _countX, int _countY)
        {
            texture = _texture;
            layout = new Layout(_texture.width, _texture.height, _startU, _startV, _endU, _endV, _countX, _countY);
        }

        public TextureLayout(string _texture, int _startU, int _startV, int _endU, int _endV, int _countX, int _countY)
        {
            texture = Vertexer.LoadTexture(_texture);
            layout = new Layout(texture.width, texture.height, _startU, _startV, _endU, _endV, _countX, _countY);
        }

        public TextureLayout(Texture _texture, Layout _layout)
        {
            texture = _texture;
            layout = _layout;
        }

        public TextureLayout(string _texture, int _countX, int _countY)
        {
            texture = Vertexer.LoadTexture(_texture);
            layout = new Layout(texture.width, texture.height, _countX, _countY);
        }

        public TextureLayout(Texture _texture, int _countX, int _countY)
        {
            texture = _texture;
            layout = new Layout(texture.width, texture.height, _countX, _countY);
        }

        public virtual void Render(long _stepCount)
        {
            var (startU, startV, endU, endV) = layout.GetFrameUvProportions(_stepCount);

            GL.PushMatrix();
            Vertexer.BindTexture(texture);
            Vertexer.DrawSquare(-1, -1,
                1, 1, startU, startV, endU, endV);
            GL.PopMatrix();
        }
    }
}
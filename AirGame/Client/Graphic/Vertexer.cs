using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class Vertexer
    {
        public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static void EnableTextures()
        {
            GL.Enable(EnableCap.Texture2D);
        }

        public static void DisableTextures()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        public static void StartDrawingQuads()
        {
            GL.Begin(PrimitiveType.Quads);
        }

        public static void StartDrawing(PrimitiveType _type)
        {
            GL.Begin(_type);
        }

        public static void VertexAt(double _x, double _y)
        {
            GL.Vertex2(_x, _y);
        }

        public static void VertexAt(double _x, double _y, double _z)
        {
            GL.Vertex3(_x, _y, _z);
        }

        public static void VertexWithUvAt(double _x, double _y, double _u, double _v)
        {
            GL.TexCoord2(_u, _v);
            GL.Vertex2(_x, _y);
        }

        public static void VertexWithUvAt(double _x, double _y, double _z, double _u, double _v)
        {
            GL.TexCoord2(_u, _v);
            GL.Vertex3(_x, _y, _z);
        }

        public static void Draw()
        {
            GL.End();
        }

        public static Texture LoadTexture(string _path)
        {
            lock (textures)
            {
                if (textures.ContainsKey(_path))
                    return textures[_path];
                var texture = new Texture("textures/" + _path);
                textures.Add(_path, texture);
                return texture;
            }
        }

        public static void DrawTexturedModalRect(Texture _texture, double _x, double _y, double _u, double _v,
            double _width,
            double _height)
        {
            _texture.Bind();
            var textureLeft = _x;
            var textureUp = _y;
            var textureRight = _x + _width;
            var textureDown = _y + _height;

            var uvLeft = _u / _texture.width;
            var uvUp = _v / _texture.height;
            var uvRight = (_u + _width) / _texture.width;
            var uvDown = (_v + _height) / _texture.height;

            StartDrawingQuads();

            VertexWithUvAt(textureLeft, textureUp, uvLeft, uvUp);
            VertexWithUvAt(textureRight, textureUp, uvRight, uvUp);
            VertexWithUvAt(textureRight, textureDown, uvRight, uvDown);
            VertexWithUvAt(textureLeft, textureDown, uvLeft, uvDown);

            Draw();
        }

        public static void BindTexture(Texture _text)
        {
            _text.Bind();
        }
        public static void BindTexture(string _text)
        {
            LoadTexture(_text).Bind();
        }
    }
}
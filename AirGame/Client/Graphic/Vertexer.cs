using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class Vertexer
    {
        public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static Texture Null { get; private set; }

        public static Color4 color { get; private set; } = Color4.White;
        public static ColorAdditionMode ColorMode { get; private set; } = 0;

        public enum ColorAdditionMode
        {
            HalfSum,
            Override,
            OnlyFirst
        }

        public static void SetColorMode(ColorAdditionMode _mode)
        {
            ColorMode = _mode;
        }

        public static void Colorize(Color4 _color)
        {
            switch (ColorMode)
            {
                case ColorAdditionMode.HalfSum:
                    color = new Color4((color.R + _color.R) / 2, (color.G + _color.G) / 2, (color.B + _color.B) / 2,
                        (color.A + _color.A) / 2);
                    break;
                case ColorAdditionMode.Override:
                    color = _color;
                    break;
                case ColorAdditionMode.OnlyFirst:
                    if (color == Color4.White)
                        color = _color;
                    break;
            }
        }

        public static void Colorize(float _r, float _g, float _b, float _a)
        {
            switch (ColorMode)
            {
                case ColorAdditionMode.HalfSum:
                    color = new Color4((color.R + _r) / 2, (color.G + _g) / 2, (color.B + _b) / 2, (color.A + _a) / 2);
                    break;
                case ColorAdditionMode.Override:
                    color = new Color4(_r, _g, _b, _a);
                    break;
                case ColorAdditionMode.OnlyFirst:
                    if (color == Color4.White)
                        color = new Color4(_r, _g, _b, _a);
                    break;
            }
        }

        public static void ClearColor()
        {
            color = Color4.White;
            ColorMode = ColorAdditionMode.OnlyFirst;
        }

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
            GL.Color4(color);
            GL.Begin(PrimitiveType.Quads);
        }

        public static void StartDrawing(PrimitiveType _type)
        {
            GL.Color4(color);
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
            GL.Color4(Color4.White);
        }

        public static Texture LoadTexture(string _path)
        {
            lock (textures)
            {
                if (textures.ContainsKey(_path))
                    return textures[_path];
                Texture texture;
                try
                {
                    texture = new Texture("textures/" + _path);
                    textures.Add(_path, texture);
                    return texture;
                }
                catch (Exception e)
                {
                    if (_path != "null.png")
                    {
                        if (Null is null)
                            Null = LoadTexture("null.png");
                        texture = Null;
                        textures.Add(_path, texture);
                        return texture;
                    }

                    throw;
                }
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
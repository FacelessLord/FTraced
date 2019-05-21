using System;
using System.Collections.Generic;
using GlLib.Client.Api.Sprites;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class Vertexer
    {
        public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static Texture Null { get; private set; }

        public static Color4 Color { get; private set; } = Color4.White;
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
                    Color = new Color4((Color.R + _color.R) / 2, (Color.G + _color.G) / 2, (Color.B + _color.B) / 2,
                        (Color.A + _color.A) / 2);
                    break;
                case ColorAdditionMode.Override:
                    Color = _color;
                    break;
                case ColorAdditionMode.OnlyFirst:
                    if (Color == Color4.White)
                        Color = _color;
                    break;
            }
        }

        public static void Colorize(float _r, float _g, float _b, float _a)
        {
            switch (ColorMode)
            {
                case ColorAdditionMode.HalfSum:
                    Color = new Color4((Color.R + _r) / 2, (Color.G + _g) / 2, (Color.B + _b) / 2, (Color.A + _a) / 2);
                    break;
                case ColorAdditionMode.Override:
                    Color = new Color4(_r, _g, _b, _a);
                    break;
                case ColorAdditionMode.OnlyFirst:
                    if (Color == Color4.White)
                        Color = new Color4(_r, _g, _b, _a);
                    break;
            }
        }

        public static void ClearColor()
        {
            Color = Color4.White;
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
            GL.Color4(Color);
            GL.Begin(PrimitiveType.Quads);
        }

        public static void StartDrawing(PrimitiveType _type)
        {
            GL.Color4(Color);
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


        public static void DrawSquare(double _sX, double _sY, double _eX, double _eY)
        {
            StartDrawing(PrimitiveType.Quads);
            VertexWithUvAt(_sX, _sY, 0, 0);
            VertexWithUvAt(_sX, _eY, 0, 1);
            VertexWithUvAt(_eX, _eY, 1, 1);
            VertexWithUvAt(_eX, _sY, 1, 0);
            Draw();
        }

        public static void DrawSquare(double _sX, double _sY, double _eX, double _eY, double _sU, double _sV,
            double _eU, double _eV)
        {
            StartDrawing(PrimitiveType.Quads);
            VertexWithUvAt(_sX, _sY, _sU, _sV);
            VertexWithUvAt(_sX, _eY, _sU, _eV);
            VertexWithUvAt(_eX, _eY, _eU, _eV);
            VertexWithUvAt(_eX, _sY, _eU, _sV);
            Draw();
        }

        public static void DrawCircle(int _accuracy = 16)
        {
            StartDrawing(PrimitiveType.TriangleFan);

            var angleStep = 2 * Math.PI / _accuracy;
            var r = 1;
            for (var i = _accuracy - 1; i >= 0; i--)
                VertexAt(r * Math.Cos(angleStep * i), r * Math.Sin(angleStep * i));

            Draw();
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

        public static void RenderAaBb(AxisAlignedBb _box, double _blockWidth, double _blockHeight)
        {
            GL.PushMatrix();
            GL.Scale(_blockWidth, _blockHeight, 1);
            BindTexture("monochromatic.png");
            StartDrawing(PrimitiveType.LineLoop);
            VertexWithUvAt(_box.startX, _box.startY, 0, 0);
            VertexWithUvAt(_box.endX, _box.startY, 1, 0);
            VertexWithUvAt(_box.endX, _box.endY, 1, 1);
            VertexWithUvAt(_box.startX, _box.endY, 0, 1);
            Draw();
            GL.PopMatrix();
        }

        public static void DrawSizedSquare(TextureLayout _layout, int _x, int _y, int _width, int _height,
            float _grainSize = 32f)
        {
            DrawSizedSquare(_layout, _x, _y, _width, _height, _grainSize, _grainSize);
        }

        public static void DrawSizedSquare(TextureLayout _layout, int _x, int _y, int _width, int _height,
            float _grainSizeX, float _grainSizeY)
        {
            var bordW = _width - _grainSizeX * 2;
            var bordH = _height - _grainSizeY * 2;
            GL.PushMatrix();
            GL.Translate(_x, _y, 0);
            DrawLayoutPart(_layout, 0, 0, 0, _grainSizeX, _grainSizeY);
            DrawLayoutPart(_layout, _grainSizeX, 0, 1, bordW, _grainSizeY);
            DrawLayoutPart(_layout, _grainSizeX + bordW, 0, 2, _grainSizeX, _grainSizeY);

            DrawLayoutPart(_layout, 0, _grainSizeY, 3, _grainSizeX, bordH);
            DrawLayoutPart(_layout, _grainSizeX, _grainSizeY, 4, bordW, bordH);
            DrawLayoutPart(_layout, _grainSizeX + bordW, _grainSizeY, 5, _grainSizeX, bordH);

            DrawLayoutPart(_layout, 0, _grainSizeY + bordH, 6, _grainSizeX, _grainSizeY);
            DrawLayoutPart(_layout, _grainSizeX, _grainSizeY + bordH, 7, bordW, _grainSizeY);
            DrawLayoutPart(_layout, _grainSizeX + bordW, _grainSizeY + bordH, 8, _grainSizeX, _grainSizeY);

            GL.PopMatrix();
        }

        public static void DrawLayoutPart(TextureLayout _layout, float _x, float _y, int _frame, float _width,
            float _height)
        {
            var (startX, startY, endX, endY) = _layout.layout.GetFrameUvProportions(_frame);

            BindTexture(_layout.texture);
            StartDrawingQuads();
            VertexWithUvAt(_x, _y, startX, startY);
            VertexWithUvAt(_width + _x, _y, endX, startY);
            VertexWithUvAt(_width + _x, _height + _y, endX, endY);
            VertexWithUvAt(_x, _height + _y, startX, endY);
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
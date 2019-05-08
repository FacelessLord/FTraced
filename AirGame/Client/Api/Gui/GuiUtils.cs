using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiUtils
    {
        public static void DrawSizedSquare(TextureLayout _layout, int _x, int _y, int _width, int _height,
            float _grainSize = 32f)
        {
            DrawSizedSquare(_layout, _x, _y, _width, _height, _grainSize, _grainSize);
        }

        public static void DrawSizedSquare(TextureLayout _layout, int _x, int _y, int _width, int _height,
            float _grainSizeX, float _grainSizeY)
        {
            GL.PushMatrix();
            GL.Translate(_x, _y, 0);
            GL.Translate(0, 0, 0);
            DrawLayoutPart(_layout, 0, _grainSizeX, _grainSizeY);

            GL.PushMatrix();
            GL.Translate(_grainSizeX, 0, 0);
            DrawLayoutPart(_layout, 1, _width - 2 * _grainSizeX + 2, _grainSizeY);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width - _grainSizeX + 2, 0, 0);
            DrawLayoutPart(_layout, 2, _grainSizeX, _grainSizeY);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, _grainSizeY, 0);
            DrawLayoutPart(_layout, 3, _grainSizeX, _height - 2 * _grainSizeY + 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_grainSizeX, _grainSizeY, 0);
            DrawLayoutPart(_layout, 4, _width - 2 * _grainSizeX + 2, _height - 2 * _grainSizeY + 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width - _grainSizeX + 2, _grainSizeY, 0);
            DrawLayoutPart(_layout, 5, _grainSizeX, _height - 2 * _grainSizeY + 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, _height - _grainSizeY + 2, 0);
            DrawLayoutPart(_layout, 6, _grainSizeX, _grainSizeY);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_grainSizeX, _height - _grainSizeY + 2, 0);
            DrawLayoutPart(_layout, 7, _width - 2 * _grainSizeX + 2, _grainSizeY);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width - _grainSizeX + 2, _height - _grainSizeY + 2, 0);
            DrawLayoutPart(_layout, 8, _grainSizeX, _grainSizeY);
            GL.PopMatrix();

            GL.PopMatrix();
        }

        public static void DrawLayoutPart(TextureLayout _layout, int _frame, float _width, float _height)
        {
            (float startX, float startY, float endX, float endY) = _layout.layout.GetFrameUvProportions(_frame);

            Vertexer.BindTexture(_layout.texture);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(0, 0, startX, startY);
            Vertexer.VertexWithUvAt(_width, 0, endX, startY);
            Vertexer.VertexWithUvAt(_width, _height, endX, endY);
            Vertexer.VertexWithUvAt(0, _height, startX, endY);
            Vertexer.Draw();
        }
    }
}
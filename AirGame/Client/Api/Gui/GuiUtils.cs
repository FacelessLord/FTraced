using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiUtils
    {
        public static void DrawSizedSquare(TextureLayout _layout, int _x, int _y, int _width, int _height,
            float _grainSize = 16f)
        {
            GL.PushMatrix();
            GL.Translate(_x, _y, 0);
            GL.Translate(0, 0, 0);
            DrawLayoutPart(_layout, 0, _grainSize, _grainSize);

            GL.PushMatrix();
            GL.Translate(_grainSize, 0, 0);
            DrawLayoutPart(_layout, 1, _width - 2 * _grainSize + 2, _grainSize);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width - _grainSize + 2, 0, 0);
            DrawLayoutPart(_layout, 2, _grainSize, _grainSize);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, _grainSize, 0);
            DrawLayoutPart(_layout, 3, _grainSize, _height - 2 * _grainSize + 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_grainSize, _grainSize, 0);
            DrawLayoutPart(_layout, 4, _width - 2 * _grainSize + 2, _height - 2 * _grainSize + 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width - _grainSize + 2, _grainSize, 0);
            DrawLayoutPart(_layout, 5, _grainSize, _height - 2 * _grainSize + 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, _height - _grainSize + 2, 0);
            DrawLayoutPart(_layout, 6, _grainSize, _grainSize);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_grainSize, _height - _grainSize + 2, 0);
            DrawLayoutPart(_layout, 7, _width - 2 * _grainSize + 2, _grainSize);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width - _grainSize + 2, _height - _grainSize + 2, 0);
            DrawLayoutPart(_layout, 8, _grainSize, _grainSize);
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
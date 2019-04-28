using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiUtils
    {
        public static void DrawSizedSquare(TextureLayout _layout, int _x, int _y, int _width, int _height,
            float _grainSize)
        {
            GL.PushMatrix();
            GL.Translate(_x, _y, 0);
            GL.Translate(-1,-1,0);
            var d = 2 * _grainSize;
            GL.Scale(1d / _grainSize, 1d / _grainSize, 1);
            DrawLayoutPart(_layout, 0, 1, 1);

            GL.PushMatrix();
            GL.Translate(1, 0, 0);
            DrawLayoutPart(_layout, 1, _width*_grainSize - 2+2*d, 1);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width*_grainSize - 1+2*d, 0, 0);
            DrawLayoutPart(_layout, 2, 1, 1);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 1, 0);
            DrawLayoutPart(_layout, 3, 1, _height*_grainSize - 2+2*d);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(1, 1, 0);
            DrawLayoutPart(_layout, 4, _width*_grainSize - 2+2*d, _height*_grainSize -  2+2*d);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width*_grainSize - 1+2*d, 1, 0);
            DrawLayoutPart(_layout, 5, 1, _height*_grainSize -  2+2*d);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, _height*_grainSize - 1+2*d, 0);
            DrawLayoutPart(_layout, 6, 1, 1);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(1, _height*_grainSize - 1+2*d, 0);
            DrawLayoutPart(_layout, 7, _width*_grainSize -2+2*d, 1);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(_width*_grainSize - 1+2*d, _height*_grainSize - 1+2*d, 0);
            DrawLayoutPart(_layout, 8, 1, 1);
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
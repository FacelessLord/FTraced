using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiUtils
    {
        public static void RenderAaBb(AxisAlignedBb _box, double _blockWidth, double _blockHeight)
        {
            GL.PushMatrix();
            GL.Scale(_blockWidth, _blockHeight,1);
            Vertexer.BindTexture("monochromatic.png");
            Vertexer.StartDrawing(PrimitiveType.LineLoop);
            Vertexer.VertexWithUvAt(_box.startX, _box.startY, 0, 0);
            Vertexer.VertexWithUvAt(_box.endX, _box.startY, 1, 0);
            Vertexer.VertexWithUvAt(_box.endX, _box.endY, 1, 1);
            Vertexer.VertexWithUvAt(_box.startX, _box.endY, 0, 1);
            Vertexer.Draw();
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
            Vertexer.BindTexture(_layout.texture);
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

            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(_x, _y, startX, startY);
            Vertexer.VertexWithUvAt(_width + _x, _y, endX, startY);
            Vertexer.VertexWithUvAt(_width + _x, _height + _y, endX, endY);
            Vertexer.VertexWithUvAt(_x, _height + _y, startX, endY);
            Vertexer.Draw();
        }
    }
}
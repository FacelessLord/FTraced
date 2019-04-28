using GlLib.Client.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiHorizontalBar : GuiObject
    {
        public GuiHorizontalBar(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
        }

        public GuiHorizontalBar(int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width, _height,
            _color)
        {
        }

        public double value = 100;
        public double maxValue = 100;

        public override void Update(GameWindow _window)
        {
            base.Update(_window);
        }

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            var start = Vertexer.LoadTexture("bar_start.png");
            var center = Vertexer.LoadTexture("bar_center.png");
            var end = Vertexer.LoadTexture("bar_end.png");
            var filler = Vertexer.LoadTexture("bar_filler.png");

            Vertexer.BindTexture(start);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x, y, 0, 0);
            Vertexer.VertexWithUvAt(x + height, y, 1, 0);
            Vertexer.VertexWithUvAt(x + height, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x, y + height, 0, 1);
            Vertexer.Draw();

            Vertexer.BindTexture(center);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x + height, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width - height, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width - height, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x + height, y + height, 0, 1);
            Vertexer.Draw();

            Vertexer.BindTexture(end);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x + width - height, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x + width - height, y + height, 0, 1);
            Vertexer.Draw();

            double proportions = value / maxValue;
            GL.Color4(color.R, color.G, color.B, color.A);
            Vertexer.BindTexture(filler);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x + height / 2, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width * proportions - height / 2d, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width * proportions - height / 2d, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x + height / 2, y + height, 0, 1);
            Vertexer.Draw();
            GL.Color4(1.0, 1, 1, 1);
        }
    }
}
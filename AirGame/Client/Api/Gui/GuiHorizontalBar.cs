using GlLib.Client.Graphic;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class GuiHorizontalBar : GuiObject
    {
        public double maxValue = 100;

        public double value = 100;

        public GuiHorizontalBar(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
        }

        public GuiHorizontalBar(int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width, _height,
            _color)
        {
        }

        public override void Update(GuiFrame _gui)
        {
            base.Update(_gui);
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            Vertexer.BindTexture(Textures.barStart);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x, y, 0, 0);
            Vertexer.VertexWithUvAt(x + height, y, 1, 0);
            Vertexer.VertexWithUvAt(x + height, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x, y + height, 0, 1);
            Vertexer.Draw();

            Vertexer.BindTexture(Textures.barCenter);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x + height, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width - height, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width - height, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x + height, y + height, 0, 1);
            Vertexer.Draw();

            Vertexer.BindTexture(Textures.barEnd);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x + width - height, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x + width - height, y + height, 0, 1);
            Vertexer.Draw();

            var proportions = value / maxValue;
            Vertexer.Colorize(color);
            var fluidWidth = (width - height) * proportions;
            Vertexer.BindTexture(Textures.barFiller);
            Vertexer.StartDrawingQuads();
            Vertexer.VertexWithUvAt(x + height / 2, y, 0, 0);
            Vertexer.VertexWithUvAt(x + fluidWidth * proportions + height / 2, y, 1, 0);
            Vertexer.VertexWithUvAt(x + fluidWidth * proportions + height / 2, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x + height / 2, y + height, 0, 1);
            Vertexer.Draw();
            Vertexer.ClearColor();
        }
    }
}
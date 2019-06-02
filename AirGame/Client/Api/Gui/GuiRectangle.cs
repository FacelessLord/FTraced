using System;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Io;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Gui
{
    public class GuiRectangle : GuiObject
    {
        public TextureLayout background;
        public float grainSize = 16f;

        public GuiRectangle(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            var texture = Vertexer.LoadTexture("gui/window_back.png");
            background = new TextureLayout(texture, 0, 0, 96, 96, 3, 3);
        }

        public GuiRectangle(int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            var texture = Vertexer.LoadTexture("gui/window_back.png");
            background = new TextureLayout(texture, 0, 0, 96, 96, 3, 3);
        }

        public GuiRectangle(TextureLayout _layout, int _x, int _y, int _width, int _height) : base(_x, _y, _width,
            _height)
        {
            background = _layout;
        }

        public GuiRectangle(TextureLayout _layout, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            background = _layout;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            Vertexer.Colorize(color);
            try
            {
                Vertexer.DrawSizedSquare(background, x, y, width, height, grainSize);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }

            Vertexer.ClearColor();

            GL.PopMatrix();
        }
    }
}
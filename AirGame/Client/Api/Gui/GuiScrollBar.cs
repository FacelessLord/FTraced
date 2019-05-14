using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiScrollBar : GuiObject
    {
        public int maximum;
        public int maxValue;

        public TextureLayout scrollBar;
        public TextureLayout scroller;
        public int scrollerPos;

        public GuiScrollBar(int _maxValue, int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            scrollBar = new TextureLayout("gui/scroll_bar.png", 3, 3);
            scroller = new TextureLayout("gui/scroller.png", 3, 3);
            maximum = height - 7 * width / 3;
            maxValue = _maxValue;
        }

        public GuiScrollBar(int _maxValue, int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width,
            _height,
            _color)
        {
            scrollBar = new TextureLayout("gui/scroll_bar.png", 3, 3);
            scroller = new TextureLayout("gui/scroller.png", 3, 3);
            maximum = height - 7 * width / 3;
            maxValue = _maxValue;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);

            GuiUtils.DrawSizedSquare(scrollBar, x, y, width, height, width / 3f, width);
            GL.Translate(0, scrollerPos + 2 * width / 3, 0);
            GuiUtils.DrawSizedSquare(scroller, x, y, width, width, width / 3f);


            GL.Color4(1.0, 1, 1, 1);
            GL.PopMatrix();
        }

        public int GetValue()
        {
            return maxValue * scrollerPos / maximum;
        }

        public override AxisAlignedBb GetObjectBox(GuiFrame _gui)
        {
            return new AxisAlignedBb(x + width / 3, y + scrollerPos + width,
                x + 2 * width / 3, y + scrollerPos + 2 * width);
        }

        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            return this;
        }

        public override void OnMouseDrag(GuiFrame _gui, int _mouseX, int _mouseY, int _dx, int _dy)
        {
            base.OnMouseDrag(_gui, _mouseX, _mouseY, _dx, _dy);
            scrollerPos += _dy;
            if (scrollerPos < 0)
                scrollerPos = 0;
            if (scrollerPos > maximum)
                scrollerPos = maximum;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiPanel : GuiObject
    {
        public List<GuiObject> screenObjects;
        public int dx = 0;
        public int dy = 0;
        public GuiScrollBar bar;
        public bool enableBackground = true;

        public GuiPanel(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            screenObjects = new List<GuiObject>();
            rectangleLayout = new TextureLayout("gui/window_back.png", 0, 0, 96, 96, 3, 3);
//            bar = new GuiScrollBar(_width - 50, 0, 50, _height);
//            bar.maximum = _height - 7 * bar.width / 3;
        }

        public TextureLayout rectangleLayout;

        public GuiPanel(int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width, _height, _color)
        {
            screenObjects = new List<GuiObject>();
        }

        public override void Update(GameWindow _window)
        {
            if (bar != null)
            {
                dy = bar.GetValue();
                bar.Update(_window);
            }

            foreach (var obj in screenObjects)
            {
                obj.Update(_window);
            }
        }

        public static int d = 4;

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            int centerX = width / 2;
            int centerY = height / 2;
            var box = GetViewbox();
            GL.PushMatrix();
            GL.Translate(x, y, 0);
            if (bar != null)
            {
                if (enableBackground)
                    GuiUtils.DrawSizedSquare(rectangleLayout, -d, -d, width - bar.width + 8 - d, height + 8 - d, 32);
                bar.Render(_window, centerX, centerY);
            }
            else
            {
                if (enableBackground)
                    GuiUtils.DrawSizedSquare(rectangleLayout, -d, -d, width + 8 - d, height + 8 - d, 32);
            }

            GL.Translate(-dx, -dy, 0);
            foreach (var obj in screenObjects)
            {
                var objRight = new PlanarVector(obj.x + obj.width, obj.y + obj.height);
                var objLeft = new PlanarVector(obj.x, obj.y);
                if (box.IsVectorInside(objRight) && box.IsVectorInside(objLeft))
                    obj.Render(_window, centerX, centerY);
            }

            GL.PopMatrix();
        }


        public override void OnMouseClick(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            foreach (var obj in screenObjects)
            {
                if (obj.IsMouseOver(_window, -x + _mouseX, -y + _mouseY))
                    obj.OnMouseClick(_window, _button, -x + _mouseX, -y + _mouseY);
            }

            if (bar != null)
                if (bar.IsMouseOver(_window, -x + _mouseX, -y + _mouseY))
                    bar.OnMouseClick(_window, _button, -x + _mouseX, -y + _mouseY);

        }

        public override void OnMouseDrag(GameWindow _window, int _mouseX, int _mouseY, int _dx, int _dy)
        {
            foreach (var obj in screenObjects)
            {
                if (obj.IsMouseOver(_window, -x + _mouseX, -y + _mouseY))
                    obj.OnMouseDrag(_window, -x + _mouseX, -y + _mouseY, _dx, _dy);
            }

            if (bar != null)
                if (bar.IsMouseOver(_window, -x + _mouseX, -y + _mouseY))
                    bar.OnMouseDrag(_window, -x + _mouseX, -y + _mouseY, _dx, _dy);

        }

        public override void OnMouseRelease(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            foreach (var obj in screenObjects)
            {
                if (obj.IsMouseOver(_window, -x + _mouseX, -y + _mouseY))
                    obj.OnMouseRelease(_window, _button, -x + _mouseX, -y + _mouseY);
            }

            if (bar != null)
                if (bar.IsMouseOver(_window, -x + _mouseX, -y + _mouseY))
                    bar.OnMouseRelease(_window, _button, -x + _mouseX, -y + _mouseY);

        }

        public GuiObject Add(GuiObject _obj)
        {
            screenObjects.Add(_obj);

            return _obj;
        }

        public AxisAlignedBb GetViewbox()
        {
            return new AxisAlignedBb(dx, dy, width + dx, height + dy);
        }

        public AxisAlignedBb GetPanelBox()
        {
            int minX = screenObjects.Select(_o => _o.x).Min();
            int minY = screenObjects.Select(_o => _o.y).Min();
            int maxX = screenObjects.Select(_o => _o.x + _o.width).Min();
            int maxY = screenObjects.Select(_o => _o.y + _o.height).Min();

            return new AxisAlignedBb(minX, minY, maxX, maxY);
        }
    }
}
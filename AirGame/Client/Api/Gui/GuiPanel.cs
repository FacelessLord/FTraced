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
        public static int d = 4;
        public GuiScrollBar bar;
        public int dx = 0;
        public int dy;
        public bool enableBackground = true;

        public TextureLayout rectangleLayout;
        public List<GuiObject> screenObjects;

        public GuiPanel(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            screenObjects = new List<GuiObject>();
            rectangleLayout = new TextureLayout("gui/window_back.png", 0, 0, 96, 96, 3, 3);
//            bar = new GuiScrollBar(_width - 50, 0, 50, _height);
//            bar.maximum = _height - 7 * bar.width / 3;
        }

        public GuiPanel(int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width, _height, _color)
        {
            screenObjects = new List<GuiObject>();
        }

        public override void Update(GuiFrame _gui)
        {
            if (bar != null)
            {
                dy = bar.GetValue();
                bar.Update(_gui);
            }

            foreach (var obj in screenObjects) obj.Update(_gui);
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            var centerX = width / 2;
            var centerY = height / 2;
            var box = GetViewbox();
            GL.PushMatrix();
            GL.Translate(x, y, 0);
            if (bar != null)
            {
                if (enableBackground)
                    GuiUtils.DrawSizedSquare(rectangleLayout, -d, -d, width - bar.width + 8 - d, height + 8 - d, 32);
                bar.Render(_gui, centerX, centerY);
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
                    obj.Render(_gui, centerX, centerY);
            }

            GL.PopMatrix();
        }


        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            foreach (var obj in screenObjects)
                if (obj.IsMouseOver(_gui, -x + _mouseX, -y + _mouseY))
                    obj.OnMouseClick(_gui, _button, -x + _mouseX, -y + _mouseY);

            if (bar != null)
                if (bar.IsMouseOver(_gui, -x + _mouseX, -y + _mouseY))
                    return bar.OnMouseClick(_gui, _button, -x + _mouseX, -y + _mouseY);

            return null;
        }

        public override void OnMouseDrag(GuiFrame _gui, int _mouseX, int _mouseY, int _dx, int _dy)
        {
            foreach (var obj in screenObjects)
                if (obj.IsMouseOver(_gui, -x + _mouseX, -y + _mouseY))
                    obj.OnMouseDrag(_gui, -x + _mouseX, -y + _mouseY, _dx, _dy);

            if (bar != null)
                if (bar.IsMouseOver(_gui, -x + _mouseX, -y + _mouseY))
                    bar.OnMouseDrag(_gui, -x + _mouseX, -y + _mouseY, _dx, _dy);
        }

        public override void OnMouseRelease(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            foreach (var obj in screenObjects)
                if (obj.IsMouseOver(_gui, -x + _mouseX, -y + _mouseY))
                    obj.OnMouseRelease(_gui, _button, -x + _mouseX, -y + _mouseY);

            if (bar != null)
                if (bar.IsMouseOver(_gui, -x + _mouseX, -y + _mouseY))
                    bar.OnMouseRelease(_gui, _button, -x + _mouseX, -y + _mouseY);
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
            var minX = screenObjects.Select(_o => _o.x).Min();
            var minY = screenObjects.Select(_o => _o.y).Min();
            var maxX = screenObjects.Select(_o => _o.x + _o.width).Max();
            var maxY = screenObjects.Select(_o => _o.y + _o.height).Max();

            return new AxisAlignedBb(minX, minY, maxX, maxY);
        }
    }
}
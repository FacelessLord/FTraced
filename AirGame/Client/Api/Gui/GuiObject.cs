using GlLib.Utils;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.API.Gui
{
    public abstract class GuiObject
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public Color color;

        public GuiObject(int _x, int _y, int _width, int _height)
        {
            (x, y) = (_x, _y);
            (width, height) = (_width, _height);
            color = Color.White;
        }

        public GuiObject(int _x, int _y, int _width, int _height, Color _color) : this(_x, _y, _width, _height)
        {
            color = _color;
        }

        public virtual void Update(GameWindow _window)
        {

        }

        public abstract void Render(GameWindow _window, int _centerX, int _centerY);

        public virtual AxisAlignedBb GetObjectBox(GameWindow _window)
        {
           return new AxisAlignedBb(x, y, x + width, y + height);
        }

        public virtual bool IsMouseOver(GameWindow _window, int _mouseX, int _mouseY)
        {
            var objBox = GetObjectBox(_window);
            var mouseVec = new PlanarVector(_mouseX, _mouseY);
            return objBox.IsVectorInside(mouseVec);
        }

        public virtual GuiObject OnMouseClick(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            return null;
        }

        public virtual void OnMouseDrag(GameWindow _window, int _mouseX, int _mouseY, int _dx, int _dy)
        {
        }

        public virtual void OnMouseRelease(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
        }
    }
}
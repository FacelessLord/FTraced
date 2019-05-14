using GlLib.Utils;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.API.Gui
{
    public abstract class GuiObject
    {
        public Color color;
        public int height;
        public int width;
        public int x;
        public int y;

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

        public virtual void Update(GuiFrame _gui)
        {
        }

        public abstract void Render(GuiFrame _gui, int _centerX, int _centerY);

        public virtual AxisAlignedBb GetObjectBox(GuiFrame _guiFrame)
        {
            return new AxisAlignedBb(x, y, x + width, y + height);
        }

        public virtual bool IsMouseOver(GuiFrame _guiFrame, int _mouseX, int _mouseY)
        {
            var objBox = GetObjectBox(_guiFrame);
            var mouseVec = new PlanarVector(_mouseX, _mouseY);
            return objBox.IsVectorInside(mouseVec);
        }

        public virtual GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            return null;
        }

        public virtual void OnMouseDrag(GuiFrame _gui, int _mouseX, int _mouseY, int _dx, int _dy)
        {
        }

        public virtual void OnMouseRelease(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
        }

        public virtual void OnKeyTyped(GuiFrame _guiFrame, KeyPressEventArgs _e)
        {
        }

        public virtual bool UnfocusOnRelease()
        {
            return true;
        }

        public virtual void OnKeyDown(GuiFrame _guiFrame, KeyboardKeyEventArgs _keyboardKeyEventArgs)
        {
        }
    }
}
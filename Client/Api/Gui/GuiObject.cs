using OpenTK;

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
    }
}
using OpenTK;

namespace GlLib.Client.API.Gui
{
    public abstract class GuiObject
    {
        public int xPosition;
        public int yPosition;
        public int objWidth;
        public int objHeight;

        public GuiObject(int _x, int _y, int _width, int _height)
        {
            (xPosition, yPosition) = (_x, _y);
            (objWidth, objHeight) = (_width, _height);
        }

        public abstract void Update(GameWindow _window);

        public abstract void Render(GameWindow _window, int _centerX, int _centerY);
    }
}
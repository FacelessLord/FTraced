using System.Collections.Generic;
using OpenTK;

namespace GlLib.Client.API.Gui
{
    public abstract class Gui
    {
        public static List<GuiObject> screenObjects;

        public Gui()
        {
            screenObjects = new List<GuiObject>();
        }

        public void Add(GuiObject _obj)
        {
            screenObjects.Add(_obj);
        }

        public void Render(GameWindow _window)
        {
            int centerX = _window.Width / 2;
            int centerY = _window.Height / 2;

            foreach (var obj in screenObjects)
            {
                obj.Render(_window,centerX,centerY);
            }
        }
    }
}
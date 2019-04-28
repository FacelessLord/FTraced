using System.Collections.Generic;
using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK;

namespace GlLib.Client.API.Gui
{
    public class Gui
    {
        public List<GuiObject> screenObjects;

        public Gui()
        {
            screenObjects = new List<GuiObject>();
        }

        public void Add(GuiObject _obj)
        {
            SidedConsole.WriteLine("" + _obj);
            screenObjects.Add(_obj);

        }

        public void AddRectangle(int _x, int _y, int _width, int _height)
        {
            Add(new GuiRectangle(_x, _y, _width, _height));
        }

        public void AddRectangle(int _x, int _y, int _width, int _height, Color _color)
        {
            Add(new GuiRectangle(_x, _y, _width, _height, _color));
        }

        public void AddPicture(Texture _texture, int _x, int _y, int _width, int _height)
        {
            Add(new GuiPicture(_texture, _x, _y, _width, _height));
        }

        public void AddPicture(Texture _texture, int _x, int _y, int _width, int _height, Color _color)
        {
            Add(new GuiPicture(_texture, _x, _y, _width, _height, _color));
        }

        public void AddText(string _text, int _x, int _y, int _width, int _height)
        {
            Add(new GuiSign(_text, _x, _y, _width, _height));
        }

        public void AddText(string _text, int _x, int _y, int _width, int _height, Color _color)
        {
            Add(new GuiSign(_text, _x, _y, _width, _height, _color));
        }
        
        public void AddHorizontalBar(int _x, int _y, int _width, int _height)
        {
            Add(new GuiHorizontalBar(_x, _y, _width, _height));
        }

        public void AddHorizontalBar(int _x, int _y, int _width, int _height, Color _color)
        {
            Add(new GuiHorizontalBar(_x, _y, _width, _height, _color));
        }
        
        public void AddNumeric(int _x, int _y, int _width, int _height)
        {
            Add(new GuiNumeric(_x, _y, _width, _height));
        }

        public void AddNumeric(int _x, int _y, int _width, int _height, Color _color)
        {
            Add(new GuiNumeric(_x, _y, _width, _height, _color));
        }

        public void Update(GameWindow _window)
        {
            foreach (var obj in screenObjects)
            {
                obj.Update(_window);
            }
        }

        public virtual void Render(GameWindow _window)
        {
            int centerX = _window.Width / 2;
            int centerY = _window.Height / 2;

            foreach (var obj in screenObjects)
            {
                obj.Render(_window, centerX, centerY);
            }
        }
    }
}
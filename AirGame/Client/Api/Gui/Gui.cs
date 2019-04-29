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

        public GuiObject Add(GuiObject _obj)
        {
            SidedConsole.WriteLine("" + _obj);
            screenObjects.Add(_obj);
            return _obj;
        }

        public GuiRectangle AddRectangle(int _x, int _y, int _width, int _height)
        {
            return (GuiRectangle) Add(new GuiRectangle(_x, _y, _width, _height));
        }

        public GuiRectangle AddRectangle(int _x, int _y, int _width, int _height, Color _color)
        {
            return (GuiRectangle) Add(new GuiRectangle(_x, _y, _width, _height, _color));
        }

        public GuiPicture AddPicture(Texture _texture, int _x, int _y, int _width, int _height)
        {
            return (GuiPicture) Add(new GuiPicture(_texture, _x, _y, _width, _height));
        }

        public GuiPicture AddPicture(Texture _texture, int _x, int _y, int _width, int _height, Color _color)
        {
            return (GuiPicture) Add(new GuiPicture(_texture, _x, _y, _width, _height, _color));
        }

        public GuiSign AddText(string _text, int _x, int _y, int _width, int _height)
        {
            return (GuiSign) Add(new GuiSign(_text, _x, _y, _width, _height));
        }

        public GuiSign AddText(string _text, int _x, int _y, int _width, int _height, Color _color)
        {
            return (GuiSign) Add(new GuiSign(_text, _x, _y, _width, _height, _color));
        }
        
        public GuiHorizontalBar AddHorizontalBar(int _x, int _y, int _width, int _height)
        {
            return (GuiHorizontalBar) Add(new GuiHorizontalBar(_x, _y, _width, _height));
        }

        public GuiHorizontalBar AddHorizontalBar(int _x, int _y, int _width, int _height, Color _color)
        {
            return (GuiHorizontalBar) Add(new GuiHorizontalBar(_x, _y, _width, _height, _color));
        }
        
        public GuiNumeric AddNumeric(int _x, int _y, int _width, int _height)
        {
            return (GuiNumeric) Add(new GuiNumeric(_x, _y, _width, _height));
        }

        public GuiNumeric AddNumeric(int _x, int _y, int _width, int _height, Color _color)
        {
            return (GuiNumeric) Add(new GuiNumeric(_x, _y, _width, _height, _color));
        }

        public virtual void Update(GameWindow _window)
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
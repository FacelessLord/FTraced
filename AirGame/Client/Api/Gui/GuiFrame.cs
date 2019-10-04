using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Io;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiFrame
    {
        public GuiObject background;
        public GuiObject focusedObject;

        public GuiFrame()
        {
            ScreenObjects = new List<GuiObject>();
        }

        public List<GuiObject> ScreenObjects { get; set; }

        public Slot SelectedSlot { get; set; }
        public bool NoClose { get; set; }

        public T Add<T>(T _obj) where T : GuiObject
        {
            ScreenObjects.Add(_obj);
            return _obj;
        }

        public GuiRectangle AddRectangle(int _x, int _y, int _width, int _height)
        {
            return Add(new GuiRectangle(_x, _y, _width, _height));
        }

        public GuiRectangle AddRectangle(int _x, int _y, int _width, int _height, Color _color)
        {
            return Add(new GuiRectangle(_x, _y, _width, _height, _color));
        }

        public GuiPicture AddPicture(Texture _texture, int _x, int _y, int _width, int _height)
        {
            return Add(new GuiPicture(_texture, _x, _y, _width, _height));
        }

        public GuiPicture AddPicture(Texture _texture, int _x, int _y, int _width, int _height, Color _color)
        {
            return Add(new GuiPicture(_texture, _x, _y, _width, _height, _color));
        }

        public GuiSign AddText(string _text, int _fontSize, int _x, int _y, int _width, int _height)
        {
            return Add(new GuiSign(_text, _fontSize, _x, _y, _width, _height));
        }

        public GuiSign AddText(string _text, int _fontSize, int _x, int _y, int _width, int _height, Color _color)
        {
            return Add(new GuiSign(_text, _fontSize, _x, _y, _width, _height, _color));
        }

        public GuiHorizontalBar AddHorizontalBar(int _x, int _y, int _width, int _height)
        {
            return Add(new GuiHorizontalBar(_x, _y, _width, _height));
        }

        public GuiHorizontalBar AddHorizontalBar(int _x, int _y, int _width, int _height, Color _color)
        {
            return Add(new GuiHorizontalBar(_x, _y, _width, _height, _color));
        }

        public GuiNumeric AddNumeric(int _x, int _y, int _width, int _height)
        {
            return Add(new GuiNumeric(_x, _y, _width, _height));
        }

        public GuiNumeric AddNumeric(int _x, int _y, int _width, int _height, Color _color)
        {
            return Add(new GuiNumeric(_x, _y, _width, _height, _color));
        }

        public virtual void Update(GameWindow _window)
        {
            foreach (var obj in ScreenObjects) obj.Update(this);
        }

        public virtual void Render(GameWindow _window)
        {
            var centerX = _window.Width / 2;
            var centerY = _window.Height / 2;

            ScreenObjects.ForEach(_o => _o.Render(this, centerX, centerY));
        }

        public virtual void OnMouseClick(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            var oldFocusedObject = focusedObject;
            foreach (var obj in ScreenObjects)
                if (obj.IsMouseOver(this, _mouseX, _mouseY))
                    focusedObject = obj.OnMouseClick(this, _button, _mouseX, _mouseY);
            oldFocusedObject?.OnMouseRelease(this, _button, _mouseX, _mouseY);
        }

        public virtual void OnKeyTyped(GameWindow _window, KeyPressEventArgs _keyEvent)
        {
            focusedObject?.OnKeyTyped(this, _keyEvent);
        }

        public virtual void OnMouseDrag(GameWindow _window, int _mouseX, int _mouseY, int _dx, int _dy)
        {
            focusedObject?.OnMouseDrag(this, _mouseX, _mouseY, _dx, _dy);
        }

        public virtual void OnMouseRelease(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
//            SidedConsole.WriteLine(focusedObject);
            if (focusedObject != null && focusedObject.UnfocusOnRelease())
            {
                focusedObject.OnMouseRelease(this, _button, _mouseX, _mouseY);
                focusedObject = null;
            }

            ScreenObjects.Where(_o => _o != focusedObject)
                .Where(_o => _o.IsMouseOver(this, _mouseX, _mouseY))
                .ToList()
                .ForEach(_o => _o.OnMouseRelease(this, _button, _mouseX, _mouseY));
        }

        public virtual void OnKeyDown(GameWindow _window, KeyboardKeyEventArgs _e)
        {
            focusedObject?.OnKeyDown(this, _e);
        }

        public GuiFrame SetNoClose(bool _noClose = true)
        {
            NoClose = _noClose;
            return this;
        }
    }
}
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Io;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiSign : GuiObject
    {
        public int fontSize = 16;
        public static FontSprite font;
        public string text;

        public GuiSign(string _text, int _fontSize, int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            text = _text;
            fontSize = _fontSize;
            font = FontSprite.Alagard;
        }

        public GuiSign(string _baseText, int _fontSize, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            fontSize = _fontSize;
            text = _baseText;
            font = FontSprite.Alagard;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            var widthCenter = (width - font.GetTextWidth(text, 11)) / 2;
            var heightCenter = (height - 11d) / 2;
            GL.PushMatrix();
            Vertexer.Colorize(color);
            GL.Translate(x + widthCenter, y + heightCenter, 0);
            font.DrawText(text, fontSize);
            Vertexer.ClearColor();

            GL.PopMatrix();
        }
    }
    
    public class GuiSlotSign : GuiObject
    {
        public int fontSize = 16;
        public static FontSprite font;
        public string text;
        public IInventory inventory;
        public int slot;

        public GuiSlotSign(IInventory _inv, int _fontSize, int _slot, int _x, int _y, int _width, int _height) : base(_x, _y, _width,
            _height)
        {
            fontSize = _fontSize;
            inventory = _inv;
            slot = _slot;
            var stack = _inv.GetStackInSlot(_slot);
            text = stack is null ? "" : stack.item.GetName(stack);
            font = FontSprite.Alagard;
        }

        public GuiSlotSign(IInventory _inv, int _slot, int _x, int _y, int _width, int _height, Color _color)
            : base(_x, _y, _width, _height, _color)
        {
            inventory = _inv;
            slot = _slot;
            var stack = _inv.GetStackInSlot(_slot);
            text = stack is null ? "" : stack.item.GetName(stack);
            font = FontSprite.Alagard;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            var widthCenter = (width - font.GetTextWidth(text, 11)) / 2;
            var heightCenter = (height - 11d) / 2;
            GL.PushMatrix();
            Vertexer.Colorize(color);
            GL.Translate(x + widthCenter, y + heightCenter, 0);
            font.DrawText(text, fontSize);
            Vertexer.ClearColor();

            GL.PopMatrix();
        }

        public override void Update(GuiFrame _gui)
        {
            base.Update(_gui);
            var stack = inventory.GetStackInSlot(slot);
            text = stack is null ? "" : stack.item.GetName(stack);
        }
    }

    public class GuiText : GuiSign
    {
        public int cursorX;

        public bool oneLineMode = true;
        public int timer;

        public GuiText(string _baseText, int _fontSize, int _x, int _y, int _width, int _height) : base(_baseText, _fontSize, _x, _y, _width,
            _height)
        {
            cursorX = _baseText.Length;
        }

        public GuiText(string _baseText, int _fontSize, int _x, int _y, int _width, int _height, Color _color) : base(_baseText, _fontSize, _x,
            _y, _width, _height, _color)
        {
            cursorX = _baseText.Length;
        }

        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            return this;
        }

        public override bool UnfocusOnRelease()
        {
            return false;
        }
        
        public static int timerSpeed = 40;

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            timer = (timer + 1) % timerSpeed;
            GL.PushMatrix();
            Vertexer.Colorize(color);
            GL.Translate(x, y, 0);
            if (oneLineMode)
            {
                var heightCenter = (height - 16d) / 2;
                GL.Translate(0, heightCenter, 0);
            }

            if (timer < timerSpeed / 2 || _gui.focusedObject != this)
            {
                font.DrawText(text, fontSize);
            }
            else if (cursorX == text.Length)
            {
                font.DrawText(text + "|", fontSize);
            }
            else
            {
                var t1 = text.Substring(0, cursorX);
                var t2 = text.Substring(cursorX);
                font.DrawText(t1 + "|" + t2, fontSize);
            }

            Vertexer.ClearColor();

            GL.PopMatrix();
        }

        public override void OnKeyTyped(GuiFrame _guiFrame, KeyPressEventArgs _e)
        {
            SidedConsole.WriteLine(_e.KeyChar);
            var k = _e.KeyChar;
            if (font.registry.ContainsKey(k))
            {
                text = text.Insert(cursorX, k + "");
                cursorX++;
            }
        }

        public virtual void HandleEnterKey()
        {
        }

        public override void OnKeyDown(GuiFrame _guiFrame, KeyboardKeyEventArgs _e)
        {
            var k = _e.Key;
            if (k == Key.BackSpace)
                if (cursorX > 0)
                {
                    if (cursorX == text.Length)
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                    else
                    {
                        var t1 = text.Substring(0, cursorX - 1);
                        var t2 = text.Substring(cursorX);
                        text = t1 + t2;
                    }

                    cursorX--;
                    timer = timerSpeed / 2 + 1;
                }

            if (k == Key.Left)
                if (cursorX > 0)
                {
                    cursorX--;
                    timer = timerSpeed / 2 + 1;
                }

            if (k.Equals(Key.Right))
                if (cursorX < text.Length)
                {
                    cursorX++;
                    timer = timerSpeed / 2 + 1;
                }

            if (k.Equals(Key.Delete))
                if (cursorX < text.Length)
                {
                    if (cursorX == 0)
                    {
                        text = text.Substring(1);
                    }
                    else
                    {
                        var t1 = text.Substring(0, cursorX);
                        var t2 = text.Substring(cursorX + 1);
                        text = t1 + t2;
                    }
                    timer = timerSpeed / 2 + 1;
                }

            if (k.Equals(Key.Enter))
            {
                if (oneLineMode)
                {
                    HandleEnterKey();
                }
                else
                {
                    text = text.Insert(cursorX, "\n");
                    cursorX++;
                }
            }
        }
    }
}
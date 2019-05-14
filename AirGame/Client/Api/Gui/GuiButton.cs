using System;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiButton : GuiObject
    {
        public static FontSprite font;
        public Action<GuiFrame, GuiButton> clickAction = (_f, _b) => { };
        public Action<GuiFrame, GuiButton, int, int> dragAction = (_f, _b, _dx, _dy) => { };
        public Action<GuiFrame, GuiButton> releaseAction = (_f, _b) => { };
        public TextureLayout spriteDisabled;
        public TextureLayout spriteEnabled;
        public TextureLayout spritePressed;
        public ButtonState state = ButtonState.Enabled;
        public string text;

        public GuiButton(string _text, int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            text = _text;
            var texture = Vertexer.LoadTexture("gui/button.png");
            var textureSelected = Vertexer.LoadTexture("gui/button_selected.png");
            var textureDisabled = Vertexer.LoadTexture("gui/button_disabled.png");
            var layout = new Layout(texture.width, texture.height, 3, 3);
            spriteEnabled = new TextureLayout(texture, layout);
            spritePressed = new TextureLayout(textureSelected, layout);
            spriteDisabled = new TextureLayout(textureDisabled, layout);
            font = new AlagardFontSprite();
        }

        public GuiButton(string _text, int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width,
            _height, _color)
        {
            text = _text;
            var texture = Vertexer.LoadTexture("gui/button.png");
            var textureSelected = Vertexer.LoadTexture("gui/button_selected.png");
            var textureDisabled = Vertexer.LoadTexture("gui/button_disabled.png");
            var layout = new Layout(texture.width, texture.height, 3, 3);
            spriteEnabled = new TextureLayout(texture, layout);
            spritePressed = new TextureLayout(textureSelected, layout);
            spriteDisabled = new TextureLayout(textureDisabled, layout);
            font = new AlagardFontSprite();
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();

            switch (state)
            {
                case ButtonState.Enabled:
                    GuiUtils.DrawSizedSquare(spriteEnabled, x, y, width, height, 16);
                    break;
                case ButtonState.Pressed:
                    GuiUtils.DrawSizedSquare(spritePressed, x, y, width, height, 16);
                    break;
                case ButtonState.Disabled:
                    GuiUtils.DrawSizedSquare(spriteDisabled, x, y, width, height, 16);
                    break;
            }

            var widthCenter = (width - font.GetTextWidth(text, 11)) / 2;
            var heightCenter = (height - 11d) / 2 - 2;
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);
            GL.Translate(x + widthCenter, y + heightCenter, 0);
            font.DrawText(text, 11);
            GL.Color4(1.0, 1, 1, 1);
            GL.PopMatrix();

            GL.PopMatrix();
//            SidedConsole.WriteLine("Render Ended");
        }

        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            base.OnMouseClick(_gui, _button, _mouseX, _mouseY);
            if (state == ButtonState.Enabled)
            {
                clickAction(_gui, this);
                state = ButtonState.Pressed;
            }

            return this;
        }

        public override void OnMouseDrag(GuiFrame _gui, int _mouseX, int _mouseY, int _dx, int _dy)
        {
            base.OnMouseDrag(_gui, _mouseX, _mouseY, _dx, _dy);
            dragAction(_gui, this, _dx, _dy);
//            x += _dx;
//            y += _dy;
        }

        public override void OnMouseRelease(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            base.OnMouseRelease(_gui, _button, _mouseX, _mouseY);
            if (state == ButtonState.Pressed)
            {
                releaseAction(_gui, this);
                state = ButtonState.Enabled;
            }
        }
    }

    public enum ButtonState
    {
        Enabled,
        Pressed,
        Disabled
    }
}
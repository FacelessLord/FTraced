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
        public ButtonState state = ButtonState.Enabled;

        public GuiButton(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            var texture = Vertexer.LoadTexture("gui/button.png");
            var textureSelected = Vertexer.LoadTexture("gui/button_selected.png");
            var textureDisabled = Vertexer.LoadTexture("gui/button_disabled.png");
            var layout = new Layout(texture.width, texture.height, 3, 3);
            SpriteEnabled = new TextureLayout(texture, layout);
            SpritePressed = new TextureLayout(textureSelected, layout);
            SpriteDisabled = new TextureLayout(textureDisabled, layout);

        }

        public GuiButton(int _x, int _y, int _width, int _height, Color _color) : base(_x, _y, _width, _height, _color)
        {

            var texture = Vertexer.LoadTexture("gui/button.png");
            var textureSelected = Vertexer.LoadTexture("gui/button_selected.png");
            var textureDisabled = Vertexer.LoadTexture("gui/button_disabled.png");
            var layout = new Layout(texture.width, texture.height, 3, 3);
            SpriteEnabled = new TextureLayout(texture, layout);
            SpritePressed = new TextureLayout(textureSelected, layout);
            SpriteDisabled = new TextureLayout(textureDisabled, layout);
        }

        public TextureLayout SpriteEnabled;
        public TextureLayout SpritePressed;
        public TextureLayout SpriteDisabled;

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Color4(color.R, color.G, color.B, color.A);

            switch (state)
            {
                case ButtonState.Enabled:
                    GuiUtils.DrawSizedSquare(SpriteEnabled, x, y, width, height, 16);
                    break;
                case ButtonState.Pressed:
                    GuiUtils.DrawSizedSquare(SpritePressed, x, y, width, height, 16);
                    break;
                case ButtonState.Disabled:
                    GuiUtils.DrawSizedSquare(SpriteDisabled, x, y, width, height, 16);
                    break;
            }

            GL.Color4(1.0, 1, 1, 1);

            GL.PopMatrix();
        }

        public override void OnMouseClick(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            base.OnMouseClick(_window, _button, _mouseX, _mouseY);
            if (state == ButtonState.Enabled)
                state = ButtonState.Pressed;
        }

        public override void OnMouseDrag(GameWindow _window, int _mouseX, int _mouseY, int _dx, int _dy)
        {
            base.OnMouseDrag(_window, _mouseX, _mouseY, _dx, _dy);
            x += _dx;
            y += _dy;
        }

        public override void OnMouseRelease(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            base.OnMouseRelease(_window, _button, _mouseX, _mouseY);
            if (state == ButtonState.Pressed)
                state = ButtonState.Enabled;
        }
    }

    public enum ButtonState
    {
        Enabled,
        Pressed,
        Disabled
    }
}
using GlLib.Client.API.Gui;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Threading;
using GlLib.Client.Api.Cameras;
using GlLib.Client.Api.Sprites;

namespace GlLib.Client.Graphic
{
    public class GraphicWindow : GameWindow
    {
        public static GraphicWindow instance;
        public VSyncMode vSync = VSyncMode.On;
        public ClientService client;
        public int guiTimeout = 0;
        public Gui gui;
        public ICamera camera;

        public Hud hud;

        public GraphicWindow(ClientService _client, int _width, int _height, string _title) : base(_width, _height,
            GraphicsMode.Default,
            _title)
        {
            client = _client;
            MouseHandler.Setup();
            SidedConsole.WriteLine("Window constructed");
            instance = this;
            hud = new Hud();
            camera = new PlayerTrackingCamera();
            TextureLayout layout = new TextureLayout(Vertexer.LoadTexture("player_sprite.png"),0,0,256,64,16,2);
            client.player.playerSprite = new LinearSprite(layout, 22, 1);
        }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            MouseHandler.Update();
            KeyboardHandler.Update();
            hud.Update(this);
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
                Proxy.Exit = true;
            }
            base.OnUpdateFrame(_e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_e);
            KeyboardHandler.SetClicked(_e.Key, true);
            KeyboardHandler.SetPressed(_e.Key, true);
            if (KeyBinds.clickBinds.ContainsKey(_e.Key) && (bool) KeyboardHandler.ClickedKeys[_e.Key])
            {
                KeyBinds.clickBinds[_e.Key](Proxy.GetClient().player);
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs _e)
        {
            base.OnKeyUp(_e);
            KeyboardHandler.SetPressed(_e.Key, false);
        }

        protected override void OnResize(EventArgs _e)
        {
            base.OnResize(_e);
            GL.Viewport(0, 0, Width, Height);
            gui?.Update(this);
        }

        protected override void OnLoad(EventArgs _e)
        {
            base.OnLoad(_e);
            VSync = VSyncMode.On;
        }

        protected override void OnMouseDown(MouseButtonEventArgs _e)
        {
            base.OnMouseDown(_e);
            gui?.OnMouseClick(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnMouseMove(MouseMoveEventArgs _e)
        {
            base.OnMouseMove(_e);
            if((bool) MouseHandler.pressed[MouseButton.Left])
                gui?.OnMouseDrag(this, _e.X, _e.Y, _e.XDelta, _e.YDelta);
        }

        protected override void OnMouseUp(MouseButtonEventArgs _e)
        {
            base.OnMouseUp(_e);
            gui?.OnMouseRelease(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs _e)
        {
            base.OnRenderFrame(_e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);

            GL.PushMatrix();
            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

//            SidedConsole.WriteLine(client.player.Position);

            GL.PushMatrix();
            GL.Translate(Width / 2d, Height / 2d, 0);
            camera.Update(this);
            camera.PerformTranslation(this);
            Proxy.GetClient().worldRenderer.Render(000, 000);
            GL.PopMatrix();

            //GUI render is not connected to the world
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);
            GL.PushMatrix();
            GL.Scale(1d / Width, 1d / Height, 1);
            hud.Update(this);
            hud.Render(this);
            gui?.Update(this);
            gui?.Render(this);
            GL.PopMatrix();

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs _e)
        {
            foreach (var key in Vertexer.textures.Keys) Vertexer.textures[key].Dispose();

            base.OnUnload(_e);
        }

        public static void RunWindow(ClientService _client)
        {
            var graphicThread = new Thread(() =>
                new GraphicWindow(_client, 800, 600, "Tracing of F").Run(60));
            graphicThread.Name = Side.Graphics.ToString();
            graphicThread.Start();
        }
    }
}
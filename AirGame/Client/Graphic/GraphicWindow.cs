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

namespace GlLib.Client.Graphic
{
    public class GraphicWindow : GameWindow
    {
        public static GraphicWindow instance;
        public static VSyncMode vSync = VSyncMode.On;
        public static ClientService client;
        public int guiTimeout = 0;
        public Gui gui;
        public double dx = 900;
        public double dy = 900;

        public Hud hud;

        public GraphicWindow(int _width, int _height, string _title) : base(_width, _height, GraphicsMode.Default,
            _title)
        {
            MouseHandler.Setup();
            SidedConsole.WriteLine("Window constructed");
            instance = this;
            hud = new Hud();
        }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            MouseHandler.Update();
            KeyboardHandler.Update();
            hud.Update(this);
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) Exit();
            base.OnUpdateFrame(_e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_e);
            KeyboardHandler.SetClicked(_e.Key, true);
            KeyboardHandler.SetPressed(_e.Key, true);
            if(KeyBinds.clickBinds.ContainsKey(_e.Key) && (bool) KeyboardHandler.ClickedKeys[_e.Key])
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

        protected override void OnRenderFrame(FrameEventArgs _e)
        {
            base.OnRenderFrame(_e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);

            GL.PushMatrix();
            GL.Scale(1d /Width, 1d /Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            SidedConsole.WriteLine(Proxy.GetClient().player.Position);

            GL.PushMatrix();
            GL.Translate((Proxy.GetClient().player.Position.x - 3*dx/4), 
                (dy / 30 - Proxy.GetClient().player.Position.y),0);
            Proxy.GetClient().worldRenderer.Render(dx,dy);
            GL.PopMatrix();

            //GUI render is not connected to the world
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);
            GL.PushMatrix();
            GL.Scale(1d / Width, 1d / Height, 1);
            gui?.Render(this);
            hud.Render(this);
            GL.PopMatrix();

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs _e)
        {
            foreach (var key in Vertexer.textures.Keys) Vertexer.textures[key].Dispose();

            base.OnUnload(_e);
        }

        public static void RunWindow()
        {
            var graphicThread = new Thread(() =>
                new GraphicWindow(800, 600, "GLLib").Run(60));
            graphicThread.Name = Side.Graphics.ToString();
            graphicThread.Start();
        }
    }
}
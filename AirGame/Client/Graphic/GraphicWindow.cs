using System;
using System.Threading;
using GlLib.Client.Api.Cameras;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic.Gui;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Graphic
{
    public class GraphicWindow : GameWindow
    {
        public ICamera camera;
        public bool enableHud = false;

        public GuiFrame guiFrame;

        public Hud hud;
        public bool serverStarted;

        public GraphicWindow(int _width, int _height, string _title) : base(_width, _height,
            GraphicsMode.Default,
            _title)
        {
            Proxy.RegisterWindow(this);
            MouseHandler.Setup();
            KeyBinds.Register();
            SidedConsole.WriteLine("Window constructed");
        }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            MouseHandler.Update();
            KeyboardHandler.Update();

            base.OnUpdateFrame(_e);
        }

        protected override void OnKeyPress(KeyPressEventArgs _e)
        {
            guiFrame?.OnKeyTyped(this, _e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            SidedConsole.WriteLine(_e.Key);
            base.OnKeyDown(_e);
            KeyboardHandler.SetClicked(_e.Key, true);
            KeyboardHandler.SetPressed(_e.Key, true);
            if (KeyBinds.clickBinds.ContainsKey(_e.Key) && (bool) KeyboardHandler.ClickedKeys[_e.Key])
                KeyBinds.clickBinds[_e.Key](Proxy.GetClient()?.player);

            guiFrame?.OnKeyDown(this, _e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs _e)
        {
            SidedConsole.WriteLine(_e.Key);
            base.OnKeyUp(_e);
            KeyboardHandler.SetPressed(_e.Key, false);
        }

        protected override void OnResize(EventArgs _e)
        {
            base.OnResize(_e);
            GL.Viewport(0, 0, Width, Height);
            guiFrame?.Update(this);
        }

        protected override void OnLoad(EventArgs _e)
        {
            base.OnLoad(_e);
            VSync = VSyncMode.Off;
            TryOpenGui(new GuiMainMenu());
            Core.profiler.SetState(State.MainMenu);
        }

        protected override void OnMouseDown(MouseButtonEventArgs _e)
        {
            base.OnMouseDown(_e);
            guiFrame?.OnMouseClick(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnMouseMove(MouseMoveEventArgs _e)
        {
            base.OnMouseMove(_e);
            if ((bool) MouseHandler.pressed[MouseButton.Left])
                guiFrame?.OnMouseDrag(this, _e.X, _e.Y, _e.XDelta, _e.YDelta);
        }

        protected override void OnMouseUp(MouseButtonEventArgs _e)
        {
            base.OnMouseUp(_e);
            guiFrame?.OnMouseRelease(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs _e)
        {
            base.OnRenderFrame(_e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (serverStarted)
                RenderWorld();


            GL.Clear(ClearBufferMask.DepthBufferBit);
            //GUI render is not connected to the world
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);
            GL.PushMatrix();
            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            if (enableHud)
            {
                hud.Update(this);
                hud.Render(this);
            }

            guiFrame?.Update(this);
            guiFrame?.Render(this);
            GL.PopMatrix();
            GL.Disable(EnableCap.Blend);

            SwapBuffers();
        }

        public void OnClientStarted()
        {
            hud = new Hud();
            camera = new PlayerTrackingCamera();
            serverStarted = true;
        }

        public void RenderWorld()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);

            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.PushMatrix();
            GL.Translate(Width / 2d, Height / 2d, 0);
            camera.Update(this);
            camera.PerformTranslation(this);
            Proxy.GetClient().worldRenderer.Render(000, 000);
            GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
        }

        protected override void OnUnload(EventArgs _e)
        {
            foreach (var key in Vertexer.textures.Keys) Vertexer.textures[key].Dispose();

            base.OnUnload(_e);
        }

        public static void RunWindow()
        {
            var graphicThread = new Thread(() =>
                new GraphicWindow(800, 600, "Tracing of F").Run(60));
            graphicThread.Name = Side.Graphics.ToString();
            graphicThread.Start();
        }

        public void TryOpenGui(GuiFrame _gui)
        {
            if (guiFrame == null)
            {
                guiFrame = _gui;
            }
            else
            {
                if (guiFrame.focusedObject == null)
                    guiFrame = null;
            }
        }
    }
}
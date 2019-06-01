using System;
using System.Threading;
using GlLib.Client.Api.Cameras;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic.Gui;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
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
        public bool enableHud;

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

        public bool CanMovementBeHandled()
        {
            return (guiFrame == null || guiFrame.focusedObject == null) &&
                   Proxy.GetClient() != null && Proxy.GetClient().player != null
                   && !Proxy.GetClient().player.IsDead && !Proxy.GetClient().player.state
                       .Equals(EntityState.Dead);
        }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            try
            {
                MouseHandler.Update();
                KeyboardHandler.Update();

                base.OnUpdateFrame(_e);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs _e)
        {
            try
            {
                guiFrame?.OnKeyTyped(this, _e);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            try
            {
                base.OnKeyDown(_e);
                KeyboardHandler.SetClicked(_e.Key, true);
                KeyboardHandler.SetPressed(_e.Key, true);
                if (KeyBinds.clickBinds.ContainsKey(_e.Key) && (bool) KeyboardHandler.ClickedKeys[_e.Key])
                    if (KeyBinds.clickBinds[_e.Key](Proxy.GetClient()?.player))
                        return;

                guiFrame?.OnKeyDown(this, _e);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs _e)
        {
            try
            {
                base.OnKeyUp(_e);
                KeyboardHandler.SetPressed(_e.Key, false);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnResize(EventArgs _e)
        {
            try
            {
                base.OnResize(_e);
                GL.Viewport(0, 0, Width, Height);
                guiFrame?.Update(this);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnLoad(EventArgs _e)
        {
            try
            {
                base.OnLoad(_e);
                VSync = VSyncMode.On;
                TryOpenGui(new GuiMainMenu());
                Core.profiler.SetState(State.MainMenu);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs _e)
        {
            try
            {
                base.OnMouseDown(_e);
                guiFrame?.OnMouseClick(this, _e.Button, _e.X, _e.Y);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs _e)
        {
            try
            {
                base.OnMouseMove(_e);
                if ((bool) MouseHandler.pressed[MouseButton.Left])
                    guiFrame?.OnMouseDrag(this, _e.X, _e.Y, _e.XDelta, _e.YDelta);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs _e)
        {
            try
            {
                base.OnMouseUp(_e);
                guiFrame?.OnMouseRelease(this, _e.Button, _e.X, _e.Y);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        public int FPS => sum / counter;
        public int counter = 1;
        public int sum = 50;

        protected override void OnRenderFrame(FrameEventArgs _e)
        {
            if (!(Proxy.GetClient() is null) && Proxy.GetClient().MachineTime.Second % 5 == 0)
            {
                sum = FPS;
                counter = 1;
            }
            
            sum += (int) (1 / _e.Time);
            counter++;
            try
            {
                base.OnRenderFrame(_e);
//                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                if (serverStarted)
                    RenderWorld();
                GL.DrawElements(BeginMode.Lines,2,DrawElementsType.UnsignedInt,);
                //GUI render is not connected to the world
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);
                GL.PushMatrix();
                GL.Scale(1d / Width, 1d / Height, 1);

                Vertexer.EnableTextures();
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

                if (enableHud && !(hud is null))
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
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        public void OnClientStarted()
        {
            hud = new Hud();
            camera = new PlayerTrackingCamera();
            serverStarted = true;
            enableHud = true;
        }

        public void RenderWorld()
        {
            try
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
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        protected override void OnUnload(EventArgs _e)
        {
            try
            {
                foreach (var key in Vertexer.textures.Keys) Vertexer.textures[key].Dispose();

                base.OnUnload(_e);
            }
            catch (Exception e)
            {
                SidedConsole.WriteLine(e);
            }
        }

        public static void RunWindow()
        {
            var graphicThread = new Thread(() =>
                new GraphicWindow(800, 600, "Tracing of F").Run(60));
            graphicThread.Name = Side.Graphics.ToString();
            graphicThread.Start();
        }

        public void CloseGui(bool _force = false)
        {
            if (guiFrame is null || !guiFrame.NoClose || _force)
                guiFrame = null;
        }

        public void OpenGui(GuiFrame _gui)
        {
            guiFrame = _gui;
        }

        public void TryOpenGui(GuiFrame _gui, bool _unfocus = false)
        {
            if (guiFrame == null)
            {
                guiFrame = _gui;
            }
            else
            {
                if (guiFrame.focusedObject == null)
                    CloseGui();
                else if (_unfocus)
                    guiFrame.focusedObject = null;
            }
        }
    }
}
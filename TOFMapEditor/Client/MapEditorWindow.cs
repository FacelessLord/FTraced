using GlLib.Client.Api.Cameras;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Threading;

namespace TOFMapEditor.Client
{
    public class MapEditorWindow : GameWindow
    {
        private World EditWorld { get; }
        private WorldRenderer WorldRenderer { get; }
        public ICamera camera;
        public GuiFrame guiFrame;
        public int guiTimeout = 0;

        public GuiFrame hud;
        public VSyncMode vSync = VSyncMode.On;

        public MapEditorWindow( int _width, int _height, string _title)
            : base(_width, _height, GraphicsMode.Default, _title)
        {
            EditWorld = new World("Overworld", 1, true);

            WorldManager.LoadWorld(EditWorld);

            WorldRenderer = new WorldRenderer(EditWorld);
            MouseHandler.Setup();
            SidedConsole.WriteLine("Window constructed");
            hud = new MapEditorHud();
            camera = new MapEditorCamera();
        }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            //SidedConsole.WriteLine(EditWorld.jsonObj);
            MouseHandler.Update();
            KeyboardHandler.Update();
            hud.Update(this);
            WorldRenderer.Render(0,0);

            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
                Proxy.Exit = true;
            }

            base.OnUpdateFrame(_e);
        }

        protected override void OnKeyPress(KeyPressEventArgs _e)
        {
            guiFrame?.OnKeyTyped(this, _e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_e);
            KeyboardHandler.SetClicked(_e.Key, true);
            KeyboardHandler.SetPressed(_e.Key, true);
            if (KeyBinds.clickBinds.ContainsKey(_e.Key) && (bool)KeyboardHandler.ClickedKeys[_e.Key])
                KeyBinds.clickBinds[_e.Key](Proxy.GetClient().player);
            guiFrame?.OnKeyDown(this, _e);
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
            guiFrame?.Update(this);
        }

        protected override void OnLoad(EventArgs _e)
        {
            base.OnLoad(_e);
            VSync = VSyncMode.On;
        }

        protected override void OnMouseDown(MouseButtonEventArgs _e)
        {
            base.OnMouseDown(_e);
            guiFrame?.OnMouseClick(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnMouseMove(MouseMoveEventArgs _e)
        {
            base.OnMouseMove(_e);
            if ((bool)MouseHandler.pressed[MouseButton.Left])
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



            hud.Update(this);
            hud.Render(this);


            guiFrame?.Update(this);
            guiFrame?.Render(this);
            GL.PopMatrix();
            GL.Disable(EnableCap.Blend);

            SwapBuffers();
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
            WorldRenderer.Render(000, 000);
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
            var graphicThread = new Thread(() 
                    => new MapEditorWindow(800, 600, "Tracing of F")
                        .Run(60))
                        { Name = Side.Graphics.ToString()};
            graphicThread.Start();
        }
    }
}
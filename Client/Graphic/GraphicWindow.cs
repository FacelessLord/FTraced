using System;
using System.Threading;
using GlLib.Client.API;
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
        public static VSyncMode vSync = VSyncMode.On;
        public static ClientService client;
        public int counter = 0;

        public GraphicWindow(int _width, int _height, string _title) : base(_width, _height, GraphicsMode.Default, _title)
        {
            MouseHandler.Setup(); 
            SidedConsole.WriteLine("Window constructed");
        }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            MouseHandler.Update();
            KeyboardHandler.Update();
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) Exit();
            base.OnUpdateFrame(_e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_e);
            KeyboardHandler.SetClicked(_e.Key, true);
            KeyboardHandler.SetPressed(_e.Key, true);
            //todo send ClickedPacket[Not necessary, clicks should be handled on Client side]
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

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);
            
            GL.PushMatrix();
            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Translate(Width / 2d, Height / 2d, 0);
//            GL.Scale(1 / 3d, 1 / 3d, 1);
//            SidedConsole.WriteLine(Proxy.GetClient().CurrentWorld);
            Proxy.GetClient().worldRenderer.Render(0, 0);
            GL.Translate(-Width / 2d, -Height / 2d, 0);
            var spriteTest = 
                new Sprite(Vertexer.LoadTexture("nebula.png"),0,0,800,800, 8,8);
            GL.Scale(0.25,0.25,1);
            spriteTest.Render(counter/2);
            counter = (counter + 1) % (8*8*2-3*2);
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
                new GraphicWindow(400, 300, "GLLib").Run(60));
            graphicThread.Name = Side.Graphics.ToString();
            graphicThread.Start();
        }
    }
}
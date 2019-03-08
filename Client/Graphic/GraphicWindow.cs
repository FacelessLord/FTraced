using System;
using System.Threading;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Packets;
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
        
        public GraphicWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            MouseHandler.Setup();
            SidedConsole.WriteLine("Window constructed");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            MouseHandler.Update();
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) Exit();
            base.OnUpdateFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            KeyboardHandler.SetClicked(e.Key, true);

            if (!KeyboardHandler.SetPressed(e.Key, true))
            {
                var pressedPkt = new KeyPressedPacket(client, e.Key);
                Proxy.SendPacketToServer(pressedPkt);
            }

            //todo send ClickedPacket[Not necessary, clicks should be handled on Client side]
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            KeyboardHandler.SetPressed(e.Key,false);
            
            var pressedPkt = new KeyUnpressedPacket(client, e.Key);
            Proxy.SendPacketToServer(pressedPkt);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            VSync = VSyncMode.On;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);

            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Translate(Width / 2d, Height / 2d, 0);
//            GL.Scale(1 / 3d, 1 / 3d, 1);
//            SidedConsole.WriteLine(Proxy.GetClient().CurrentWorld);
            Proxy.GetClient().CurrentWorld.Render(0, 0);
            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            foreach (var key in Vertexer.textures.Keys) Vertexer.textures[key].Dispose();

            Proxy.GetClient().CurrentWorld.SaveWorld();
            base.OnUnload(e);
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
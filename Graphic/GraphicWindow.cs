using System;
using GlLib.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Graphic
{
    public class GraphicWindow : GameWindow
    {
        public GraphicWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            MouseHandler.Setup();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            MouseHandler.Update();
            KeyboardHandler.Update();
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            Core.Core.World.Update();
            base.OnUpdateFrame(e);
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
            GL.Scale(1 / 3d, 1 / 3d, 1);
            Core.Core.World.Render(0, 0);
            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            foreach (var key in Vertexer._textures.Keys)
            {
                Vertexer._textures[key].Dispose();
            }
            Core.Core.World.UnloadWorld();
            base.OnUnload(e);
        }

        public static VSyncMode _vSync = VSyncMode.On;
    }
}
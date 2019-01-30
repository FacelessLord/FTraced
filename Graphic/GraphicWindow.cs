using System;
using System.Collections.Generic;
using System.Drawing;
using GlLib.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Color = OpenTK.Color;

namespace GlLib.Graphic
{
    public class GraphicWindow : GameWindow
    {
        public GraphicWindow(int width, int height, string title) : base(width, height,GraphicsMode.Default, title)
        {
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
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

        static GraphicWindow()
        {
            
            for(int i=0;i<16;i++)
            for (int j = 0; j < 16; j++)
                blocks[i, j] = b;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);

            GL.Scale(1d / Width, 1d / Height, 1);

            GL.Translate(0, 0, 0);
            GL.Scale(0.75, 0.75, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


            Chunk chk = new Chunk(0, 0, blocks);
            chk.RenderChunk(Width / 2, Height / 2);
            Console.WriteLine("i");
            SwapBuffers();
        }

        static TerrainBlock b = new GrassBlock();
        static TerrainBlock[,] blocks = new TerrainBlock[16, 16];

        protected override void OnUnload(EventArgs e)
        {
            foreach (var key in Vertexer.textures.Keys)
            {
                Vertexer.textures[key].Dispose();
            }
            base.OnUnload(e);
        }

        public static VSyncMode _vSync = VSyncMode.On;
    }
}
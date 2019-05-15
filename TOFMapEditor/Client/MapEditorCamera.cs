using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.Api.Cameras;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TOFMapEditor.Client
{
    internal class MapEditorCamera : ICamera
    {
        public double posX;
        public double posY;

        public void Update(GameWindow _window)
        {
            posX = 0;
            posY = 0;
        }


        public void PerformTranslation(GameWindow _window)
        {
            GL.Translate(-posX * 64, -posY * 32, 0);
        }
    }
}

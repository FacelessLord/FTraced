using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Chat;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Common.Registries;
using GlLib.Utils;
using GlLib.Utils.StringParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform.Windows;

namespace GlLib.Client.Api.Gui
{
    public class GuiChatHistory : GuiObject
    {
        public int fontSize;
        public static FontSprite font;

        public GuiChatHistory(int _fontSize, int _x, int _y, int _width, int _height) : 
            base(_x, _y, _width,_height)
        {
            fontSize = _fontSize;
            font = FontSprite.Alagard;
        }

        public GuiChatHistory(int _fontSize, int _x, int _y, int _width, int _height, Color _color) :
            base(_x, _y, _width, _height, _color)
        {
            fontSize = _fontSize;
            font = FontSprite.Alagard;
        }
        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Translate(x, y + height, 0);
            
            foreach (var line in Proxy.GetClient().player.chatIo.InputStream().Take(height*2/fontSize/3))
            {
                GL.Translate(0, -fontSize * 3d / 2, 0);
                font.DrawText(line, fontSize, _a: 0.75f);
            }

            Vertexer.ClearColor();
            GL.PopMatrix();
        }

        public override void OnKeyDown(GuiFrame _guiFrame, KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_guiFrame, _e);

            //TODO maybe clear history or copy/paste
        }
    }
}
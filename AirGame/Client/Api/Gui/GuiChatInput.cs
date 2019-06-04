using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
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
    public class GuiChatInput : GuiText
    {
        public int historyPointer = 0;
        private Parser _parser;

        public GuiChatInput(string _baseText, int _fontSize, int _x, int _y, int _width, int _height) : base(_baseText,
            _fontSize, _x, _y, _width,
            _height)
        {
            Initialize();
        }

        public GuiChatInput(string _baseText, int _fontSize, int _x, int _y, int _width, int _height, Color _color) :
            base(_baseText, _fontSize, _x, _y, _width, _height, _color)
        {
            Initialize();
        }

        private void Initialize()
        {
            _parser = new CommandParser();
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            base.Render(_gui, _centerX, _centerY);
            GL.PushMatrix();
            Vertexer.Colorize(color);
            GL.Translate(x, y, 0);
            if (oneLineMode)
            {
                var heightCenter = (height - 16d) / 2;
                GL.Translate(0, heightCenter - height / 2d, 0);
                foreach (var line in Proxy.GetClient().player.chatIo.InputStream())
                {
                    GL.Translate(0, -height * 2d / 3, 0);
                    font.DrawText(line, fontSize);
                }
            }

            Vertexer.ClearColor();
            GL.PopMatrix();
        }

        public override void OnKeyDown(GuiFrame _guiFrame, KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_guiFrame, _e);

            if (_e.Key is Key.Up)
            {
                var commands = Proxy.GetClient().player.chatIo.InputStream().Where(_l => _l.StartsWith("$> ")).ToList();
                if (historyPointer < commands.Count - 1)
                {
                    text = commands[++historyPointer].Substring(3);
                    cursorX = text.Length;
                }
            }

            if (_e.Key is Key.Down)
            {
                var commands = Proxy.GetClient().player.chatIo.InputStream().Where(_l => _l.StartsWith("$> ")).ToList();
                if (historyPointer > 0)
                {
                    text = commands[--historyPointer].Substring(3);
                    cursorX = text.Length;
                }
                else
                {
                    text = "";
                    cursorX = 0;
                    historyPointer = 0;
                }
            }
        }

        public override void HandleEnterKey()
        {
            base.HandleEnterKey();

            var io = Proxy.GetClient().player.chatIo;

            io.Output("$> " + text);
            if (text.StartsWith('/'))
                _parser.Parse(text.Substring(1), io);

            cursorX = 0;
            historyPointer = -1;
            text = "";
        }
    }
}
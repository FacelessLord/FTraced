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
            base(_x, _y, _width, _height)
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

            bool centeringMode = false;
            bool coloningMode = false;
            var formattedLines = new List<string>();
            double maxLength = 0;

            foreach (var line in Proxy.GetClient().player.chatIo.InputStream().Take(height * 2 / fontSize / 3))
            {
                if (centeringMode || line.StartsWith("^^"))
                {
                    if (!line.StartsWith("^^"))
                    {
                        foreach (var formatLine in formattedLines)
                        {
                            double length = font.GetTextWidth(formatLine, fontSize);

                            GL.Translate(maxLength / 2 - length / 2, -fontSize * 3d / 2, 0);
                            font.DrawText(formatLine.Substring(2),fontSize, _a: 0.75f);
                            GL.Translate(-maxLength / 2 + length / 2, 0, 0);
                        }
                    }
                    else
                    {
                        formattedLines.Add(line);
                        double lineLength = font.GetTextWidth(line, fontSize);
                        maxLength = maxLength < lineLength ? lineLength : maxLength;
                    }

                    centeringMode = line.StartsWith("^^");
                }
                else if (coloningMode || line.StartsWith("::"))
                {
                    if (!line.StartsWith("::"))
                    {
                        foreach (var formatLine in formattedLines)
                        {
                            string command = formatLine.Substring(2, formatLine.Substring(2).IndexOf(':'));
                            double length = font.GetTextWidth(command, fontSize);

                            GL.Translate(maxLength - length, -fontSize * 3d / 2, 0);
                            font.DrawText(formatLine.Substring(2), fontSize, _a: 0.75f);
                            GL.Translate(-maxLength + length, 0, 0);
                        }
                    }
                    else
                    {
                        formattedLines.Add(line);
                        string command = line.Substring(2, line.Substring(2).IndexOf(':'));
                        double lineLength = font.GetTextWidth(command, fontSize);
                        maxLength = maxLength < lineLength ? lineLength : maxLength;
                    }

                    coloningMode = line.StartsWith("::");
                }
                else
                {
                    GL.Translate(0, -fontSize * 3d / 2, 0);
                    font.DrawText(line, fontSize, _a: 0.75f);
                }
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
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiChatHistory : GuiObject
    {
        public static FontSprite font;
        public int fontSize;

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

            var centeringMode = false;
            var coloningMode = false;
            var formattedLines = new List<string>();
            double maxLength = 0;

            foreach (var line in Proxy.GetClient().entityPlayer.chatIo.InputStream().Take(height * 2 / fontSize / 3))
                if (centeringMode || line.StartsWith("^^"))
                {
                    if (!line.StartsWith("^^"))
                    {
                        foreach (var formatLine in formattedLines)
                        {
                            var length = font.GetTextWidth(formatLine, fontSize);

                            GL.Translate(maxLength / 2 - length / 2, -fontSize * 3d / 2, 0);
                            font.DrawText(formatLine.Substring(2), fontSize, _a: 0.75f);
                            GL.Translate(-maxLength / 2 + length / 2, 0, 0);
                        }
                    }
                    else
                    {
                        formattedLines.Add(line);
                        var lineLength = font.GetTextWidth(line, fontSize);
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
                            var command = formatLine.Substring(2, formatLine.Substring(2).IndexOf(':'));
                            var length = font.GetTextWidth(command, fontSize);

                            GL.Translate(maxLength - length, -fontSize * 3d / 2, 0);
                            font.DrawText(formatLine.Substring(2), fontSize, _a: 0.75f);
                            GL.Translate(-maxLength + length, 0, 0);
                        }
                    }
                    else
                    {
                        formattedLines.Add(line);
                        var command = line.Substring(2, line.Substring(2).IndexOf(':'));
                        var lineLength = font.GetTextWidth(command, fontSize);
                        maxLength = maxLength < lineLength ? lineLength : maxLength;
                    }

                    coloningMode = line.StartsWith("::");
                }
                else
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
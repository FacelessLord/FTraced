using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using GlLib.Client.API;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common;
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
        private Parser _parser = new Parser();

        public GuiChatInput(string _baseText, int _x, int _y, int _width, int _height) : base(_baseText, _x, _y, _width,
            _height)
        {
            Initialize();
        }

        public GuiChatInput(string _baseText, int _x, int _y, int _width, int _height, Color _color) : base(_baseText,
            _x, _y, _width, _height, _color)
        {
            Initialize();
        }

        private void Initialize()
        {
            _parser.AddParse("", (_s, _io) => { });
            _parser.AddParse("clear", (_s, _io) => _io.TryClearStream(), "Clears all output");
            _parser.AddParse("help", (_s, _io) =>
            {
                _io.Output("This is Help" + "\n"
                                          + _parser.GetCommandList());
            });

            _parser.AddParse("spawn", (_s, _io) =>
            {
                if (_s.Length == 0)
                {
                    _io.Output("What shall I spawn?");
                    return;
                }

                if (!Proxy.GetRegistry().entities.ContainsKey("entity." + _s[0]))
                {
                    _io.Output("Can't spawn entity with name: " + _s[0]);
                    return;
                }

                var entity = Proxy.GetRegistry().GetEntityFromName("entity." + _s[0]);
                entity.worldObj = Proxy.GetClient().player.worldObj;
                entity.Position = Proxy.GetClient().player.Position;
                entity.velocity = Proxy.GetClient().player.velocity.Normalized;
                Proxy.GetClient().world.SpawnEntity(entity);
            }, "Spawns entity. Use full name.");

            _parser.AddParse("lspawn", (_s, _io) =>
            {
                if (_s.Length == 0)
                {
                    _io.Output("What shall I spawn?");
                    return;
                }

                if (!Proxy.GetRegistry().entities.ContainsKey("entity.living." + _s[0]))
                {
                    _io.Output("Can't spawn entity with name: " + _s[0]);
                    return;
                }

                var entity = Proxy.GetRegistry().GetEntityFromName("entity.living." + _s[0]);
                entity.worldObj = Proxy.GetClient().player.worldObj;
                entity.Position = Proxy.GetClient().player.Position;
                entity.velocity = Proxy.GetClient().player.velocity.Normalized;
                Proxy.GetClient().world.SpawnEntity(entity);
            }, "Spawns entity.living. Use name. Example: lspawn box");
            _parser.AddParse("gm", (_s, _io) =>
            {
                if (_s.Length == 0)
                {
                    _io.Output("What game mode are you will?");
                    return;
                }

                if (_s[0] == "1")
                    Proxy.GetClient().player.SetGodMode();
                if (_s[0] == "0")
                    Proxy.GetClient().player.SetGodMode(false);
            }, "Change player's god mode.0 - if it should be on, 1 - if it should be off.");
            _parser.AddParse("setbrush", (_s, _io) =>
            {
                if (_s.Length == 0)
                {
                    _io.Output("What brush should I use?");
                    return;
                }

                int id;
                TerrainBlock block;

                if (int.TryParse(_s[0], out id))
                {
                    if (Proxy.GetRegistry().TryGetBlockFromId(id, out block))
                        Proxy.GetClient().player.Brush = block;
                    else
                        _io.Output("Wrong ID, can't chose this block.");
                }

                else if (_s[0].StartsWith("block."))
                {
                    if (Proxy.GetRegistry().TryGetBlockFromName("block." + _s[0], out block))
                        Proxy.GetClient().player.Brush = block;
                    else
                        _io.Output("Wrong block name, can't chose this block.");
                }
                else
                {
                    if (Proxy.GetRegistry().TryGetBlockFromName(_s[0], out block))
                        Proxy.GetClient().player.Brush = block;
                    else
                        _io.Output("Wrong block name, can't chose this block.");
                }
            }, "Chose block to set to.");


            _parser.AddParse("brush",
                (_s, _io) => { _io.Output($"Now brush have block {Proxy.GetClient().player.Brush.Name}"); });
            _parser.AddParse("list", (_s, _io) =>
            {
                if (_s.Length == 0)
                {
                    _io.Output("What objects do you want?");
                    return;
                }

                if (_s[0] == "items")
                {
                    var concatedItems = "Here is information 'bout items.\n";
                    concatedItems += $"{"Name",12} {"ID",15:N0}\n\n";
                    var items = Proxy.GetRegistry().itemsById;
                    foreach (var id in items.Keys)
                    {
                        concatedItems += $"{(items[id] as Item)?.ToString(),12} {id,15:N0}\n";
                    }

                    _io.Output(concatedItems);
                }
                else if (_s[0] == "blocks")
                {
                    var concatedBlocks = "Here is information 'bout blocks.\n";
                    concatedBlocks += $"{"Name",-35} {"ID",10:N0}\n\n";
                    var blocks = Proxy.GetRegistry().blocksById;
                    foreach (var id in blocks.Keys)
                    {
                        concatedBlocks += $"{(blocks[id] as TerrainBlock)?.Name,-35} {id,10:N0}\n";
                    }

                    _io.Output(concatedBlocks);
                }
                else if (_s[0] == "entities")
                {
                    var concateEntities = "Here is information 'bout entities.\n";
                    var entities = Proxy.GetRegistry().entities;
                    foreach (var name in entities.Keys)
                    {
                        concateEntities += $"{name,8} {name,20:N0}\n";
                    }

                    _io.Output(concateEntities);
                }
                else _io.Output("blocks, entities or items please");
            }, "Shows game registry.");
            _parser.AddParse("save", (_p, _io) =>
            {
                WorldManager.SaveWorld(Proxy.GetClient().player.worldObj);
                _io.Output("World Saved");
            });
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
                    font.DrawText(line, 11);
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
            _parser.Parse(text, io);

            cursorX = 0;
            historyPointer = -1;
            text = "";
        }
    }
}
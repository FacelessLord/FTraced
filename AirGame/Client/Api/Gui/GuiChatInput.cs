using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using GlLib.Client.API.Gui;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Common.Registries;
using GlLib.Utils;
using GlLib.Utils.StringParser;
using OpenTK;
using OpenTK.Platform.Windows;

namespace GlLib.Client.Api.Gui
{
    public class GuiChatInput : GuiText
    {
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
            _parser.AddParse("help", _s =>
            {
                SidedConsole.WriteLine("This is Help" + "\n"
                 + _parser.GetCommandList() );
            });

            _parser.AddParse("spawn", _s =>
            {
                if (_s.Length == 0)
                {
                    SidedConsole.WriteLine("What shall I spawn?");
                    return;
                }
                if (!Proxy.GetRegistry().entities.ContainsKey("entity." + _s[0]))
                {
                    SidedConsole.WriteLine("Can't spawn entity with name: " + _s[0]);
                    return;
                }
                var entity = Proxy.GetRegistry().GetEntityFromName("entity." + _s[0]);
                entity.worldObj = Proxy.GetClient().player.worldObj;
                entity.Position = Proxy.GetClient().player.Position;
                entity.velocity = Proxy.GetClient().player.velocity.Normalized;
                Proxy.GetClient().world.SpawnEntity(entity);
            });

            _parser.AddParse("lspawn", _s =>
            {
                if (_s.Length == 0)
                {
                    SidedConsole.WriteLine("What shall I spawn?");
                    return;
                }
                if (!Proxy.GetRegistry().entities.ContainsKey("entity.living." + _s[0]))
                {
                    SidedConsole.WriteLine("Can't spawn entity with name: " + _s[0]);
                    return;
                }
                var entity = Proxy.GetRegistry().GetEntityFromName("entity.living." + _s[0]);
                entity.worldObj = Proxy.GetClient().player.worldObj;
                entity.Position = Proxy.GetClient().player.Position;
                entity.velocity = Proxy.GetClient().player.velocity.Normalized;
                Proxy.GetClient().world.SpawnEntity(entity);
            });
            _parser.AddParse("gm",_s =>
            {
                if (_s.Length == 0)
                {
                    SidedConsole.WriteLine("What game mode are you will?");
                    return;
                }

                if (_s[0] == "1")
                    Proxy.GetClient().player.SetGodMode();
                if (_s[0] == "0")
                    Proxy.GetClient().player.SetGodMode(false);
            });
            _parser.AddParse("setbrush", _s =>
            {
                if (_s.Length == 0)
                {
                    SidedConsole.WriteLine("What brush should I use?");
                    return;
                }

                int id;
                TerrainBlock block;

                if (int.TryParse(_s[0], out id))
                {
                    if (Proxy.GetRegistry().TryGetBlockFromId(id, out block))
                        Proxy.GetClient().player.Brush = block;
                    else
                        SidedConsole.WriteLine("Wrong ID, can't chose this block.");
                }

                else if (!_s[0].StartsWith("block."))
                {
                    if (Proxy.GetRegistry().TryGetBlockFromName("block." + _s[0], out block))
                        Proxy.GetClient().player.Brush = block;
                    else
                        SidedConsole.WriteLine("Wrong block name, can't chose this block.");
                }
                else
                {
                    if (Proxy.GetRegistry().TryGetBlockFromName(_s[0], out block))
                        Proxy.GetClient().player.Brush = block;
                    else
                        SidedConsole.WriteLine("Wrong block name, can't chose this block.");
                }
            });

            _parser.AddParse("list", _s =>
            {
                if (_s.Length == 0)
                {
                    SidedConsole.WriteLine("What objects do you want?");
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
                    SidedConsole.WriteLine(concatedItems);
                    
                }
                else if (_s[0] == "blocks")
                {
                    var concatedBlocks = "Here is information 'bout blocks.\n";
                    concatedBlocks += $"{"Name",-35} {"ID",10:N0}\n\n";
                    var blocks = Proxy.GetRegistry().blocksById;
                    foreach (var id in blocks.Keys)
                    {
                        concatedBlocks += $"{(blocks[id] as TerrainBlock)?.Name ,-35} {id,10:N0}\n";
                    }
                    SidedConsole.WriteLine(concatedBlocks);
                }
                else if (_s[0] == "entities")
                {
                    var concateEntities = "Here is information 'bout entities.\n";
                    var entities = Proxy.GetRegistry().entities;
                    foreach (var name in entities.Keys)
                    {
                        concateEntities += $"{name,8} {name,20:N0}\n";
                    }
                    SidedConsole.WriteLine(concateEntities);
                }
                else SidedConsole.WriteLine("blocks, entities or items please");
            });
            _parser.AddParse("save", _p =>
            {
                WorldManager.SaveWorld(Proxy.GetClient().player.worldObj);
            });

        }


        public override void HandleEnterKey()
        {
            base.HandleEnterKey();

            _parser.Parse(text);

            cursorX = 0;
            text = "";
        }
    }
}
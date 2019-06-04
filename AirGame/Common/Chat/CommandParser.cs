using System.Xml.Linq;
using GlLib.Client.Api;
using GlLib.Common.Entities;
using GlLib.Common.Io;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Utils.StringParser;
using SixLabors.ImageSharp.MetaData.Profiles.Icc;

namespace GlLib.Common.Chat
{
    internal class CommandParser : Parser
    {
        public CommandParser()
        {
            Init();
        }
        
        public void Init()
        {
            AddParse("", (_s, _io) => { });
            AddParse("help", (_s, _io) => _io.Output("This is Help" + "\n" + GetCommandList()),
                "Prints help message");
            AddParse("clear", (_s, _io) => _io.TryClearStream(), "Clears all output");
            AddParse("spawn", (_s, _io) => Spawn(_s, _io),
                "Spawns entity. Use full name.");
            AddParse("lspawn", (_s, _io) => LivingSpawn(_s, _io),
                "Spawns entity.living. Use name. Example: lspawn box");
            AddParse("gm", (_s, _io) => ChangeGameMode(_s, _io),
                "Change player's god mode.0 - if it should be on, 1 - if it should be off.");
            AddParse("noclip", (_s, _io) => SwitchNoClip(_s, _io),
                "Switches current player noClip state");
            AddParse("setbrush", (_s, _io) => ChangeBrush(_s, _io),
                "Chose block to set to.");
            AddParse("brush",
                (_s, _io) => _io.Output($"Now brush is {Proxy.GetClient().player.Brush.Name}"),
                "Prints current brush block");
            AddParse("list", (_s, _io) => GetRegistryList(_s, _io),
                "Shows game registry.");
            AddParse("save", (_p, _io) => SaveWorld(_p, _io),
                "Saves whole world (including blocks)");
            AddParse("killchunk", (_p, _io) => KillChunk(_p, _io),
                "Kills all entities in chunk that can be killed");
        }

        public static void SwitchNoClip(string[] _s, IStringIo _io)
        {
            bool newState = !Proxy.GetClient().player.noClip;
            if (_s.Length > 0 && bool.TryParse(_s[0], out bool definedState))
            {
                Proxy.GetClient().player.noClip = definedState;
            }

            Proxy.GetClient().player.noClip = newState;

            _io.Output("Current noclip state: " + Proxy.GetClient().player.noClip);
        }

        public static void KillChunk(string[] _s, IStringIo _io)
        {
            if (_s.Length > 0 && _s[0] != "")
            {
                if (_s[0] == "~")
                {
                    var force = _s.Length > 1 && _s[1] == "-force";
                    foreach (var entity in Proxy.GetClient().player.chunkObj.entities)
                    {
                        if(entity is Player)
                            continue;
                        if (force || !(entity is EntityLiving el) || el.CanDie )
                            entity.SetDead();
                    }

                    _io.Output("Entities in current chunk has been killed");
                    return;
                }

                int cx = 0;
                int cy = 0;
                if (int.TryParse(_s[0], out cx))
                {
                    if (int.TryParse(_s[1], out cy))
                    {
                        var force = _s.Length > 1 && _s[1] == "-force";
                        var w = Proxy.GetClient().player.worldObj;
                        if (w.width > cx && 0 <= cx && w.height > cy && 0 <= cy)
                        {
                            var chunk = w[cx, cy];
                            if (chunk != null && chunk.isLoaded)
                            {
                                foreach (var entity in w[cx, cy].entities)
                                {
                                    if (force || !(entity is EntityLiving el) || el.CanDie)
                                        entity.SetDead();
                                }

                                _io.Output("Entities in chunk (" + cx + ", " + cy + ") has been killed");
                                return;
                            }
                        }
                    }
                }
                
                _io.Output("Chunk (" + cx + ", " + cy + ") doesn't exist");
            }

            _io.Output("Too few arguments.");
            _io.Output("Please specify chunk by either");
            _io.Output("- typing '~' to point to current chunk");
            _io.Output("- typing chunk's coordinates");
        }

        public static void Spawn(string[] _s, IStringIo _io)
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
        }

        public static void LivingSpawn(string[] _s, IStringIo _io)
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
        }

        public static void ChangeGameMode(string[] _s, IStringIo _io)
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
        }

        public static void ChangeBrush(string[] _s, IStringIo _io)
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

            else if (!_s[0].StartsWith("block."))
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
        }

        public static void GetRegistryList(string[] _s, IStringIo _io)

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
        }

        public static void SaveWorld(string[] _s, IStringIo _io)
        {
            WorldManager.SaveWorld(Proxy.GetClient().player.worldObj);
            _io.Output("World Saved");
        }
    }
}
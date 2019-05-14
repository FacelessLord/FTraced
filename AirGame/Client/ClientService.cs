using System;
using System.Linq;
using System.Net;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Client
{
    public class ClientService : SideService
    {
        public WorldRenderer worldRenderer;
        public World world;

        public GraphicWindow window;

        public string nickName;
        public string password;
        public volatile Player player;

        public ClientService(string _nickName, string _password) : base(Side.Client)
        {
            nickName = _nickName;
            password = _password;
            
        }

        public void UpdateRendererData(World _world)
        {
            worldRenderer = new WorldRenderer(_world);
            world = _world;
        }

        public override void OnStart()
        {
//            SidedConsole.WriteLine("Setting World");
            world = Proxy.GetServer().GetWorldById(0);
            UpdateRendererData(world);
//            SidedConsole.WriteLine("Setting Player");
            foreach (var chunk in world.chunks)
            {
                var players = chunk.entities.SelectMany(_o => _o).Where(_e => _e is Player).Cast<Player>().ToList();
                if (players.Any())
                {
                    player = players.First();
                }
            }

            if(player is null)
            {
                player = new Player();
//            SidedConsole.WriteLine("Setting Player Name");
                player.nickname = nickName;
//            SidedConsole.WriteLine("Setting Player Pos");
                player.Position = new RestrictedVector3D(world.width * 8, world.height * 8, 0);
//            SidedConsole.WriteLine("Setting Player Data");
                player.data = Proxy.GetServer().GetDataFor(player, password);
                Proxy.GetServer().GetWorldById(0).SpawnEntity(player);
            }
//            SidedConsole.WriteLine("Loading window");
            Proxy.GetWindow().OnClientStarted();
        }

        public override void OnServiceUpdate()
        {
            foreach (var bind in KeyBinds.binds)
            {
                if (KeyboardHandler.PressedKeys.ContainsKey(bind.Key) && (bool) KeyboardHandler.PressedKeys[bind.Key])
                {
                    KeyBinds.binds[bind.Key](player);
                }
            }
            //bad idea to update it on client side
            player.spells.OnUpdate();
        }

        public override void OnExit()
        {
        }
    }
}
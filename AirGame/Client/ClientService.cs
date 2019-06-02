using System.Linq;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Entities.Items;
using GlLib.Common.Map;
using GlLib.Utils;
using GlLib.Utils.Math;

namespace GlLib.Client
{
    public class ClientService : SideService
    {
        public string nickName;
        public string password;
        public volatile Player player;

        public GraphicWindow window;
        public World world;
        public WorldRenderer worldRenderer;

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
                var players = chunk.entities.Where(_e => _e is Player).Cast<Player>().ToList();
                if (players.Any()) player = players.First();
            }

            if (player is null) ResurrectPlayer();
            var coin = new Coin();
            coin.Position = new RestrictedVector3D(world.width * 8, world.height * 8, 0);
            world.SpawnEntity(coin);

//            SidedConsole.WriteLine("Loading window");
            Proxy.GetWindow().OnClientStarted();
        }

        public void ResurrectPlayer()
        {
            if (player is null || player.state is EntityState.Dead)
            {
                if (player.state is EntityState.Dead)
                    player.SetDead();
                player = new Player();
//            SidedConsole.WriteLine("Setting Player Name");
                player.nickname = nickName;
//            SidedConsole.WriteLine("Setting Player Pos");
                player.Position = new RestrictedVector3D(world.width * 8, world.height * 8, 0);
//            SidedConsole.WriteLine("Setting Player Data");
                player.data = Proxy.GetServer().GetDataFor(player, password);
                Proxy.GetServer().GetWorldById(0).SpawnEntity(player);
            }
        }

        public override void OnServiceUpdate()
        {
            foreach (var bind in KeyBinds.binds)
                if (KeyboardHandler.PressedKeys.ContainsKey(bind.Key) && (bool) KeyboardHandler.PressedKeys[bind.Key])
                    KeyBinds.binds[bind.Key](player);
            //bad idea to update it on client side
            player.spells.OnUpdate();
        }

        public override void OnExit()
        {
        }
    }
}
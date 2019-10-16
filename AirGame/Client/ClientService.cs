using System.Linq;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Entities.Items;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Client
{
    public class ClientService : SideService
    {
        public string nickName;
        public string password;
        public volatile EntityPlayer entityPlayer;

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
                var players = chunk.entities.Where(_e => _e is EntityPlayer).Cast<EntityPlayer>().ToList();
                if (players.Any()) entityPlayer = players.First();
            }

            if (entityPlayer is null) ResurrectPlayer();
            var coin = new Coin();
            coin.Position = new RestrictedVector3D(world.width * 8, world.height * 8);
            world.SpawnEntity(coin);

//            SidedConsole.WriteLine("Loading window");
            Proxy.GetWindow().OnClientStarted();
        }

        public void ResurrectPlayer()
        {
            if (entityPlayer is null || entityPlayer.state is EntityState.Dead)
            {
                if (entityPlayer.state is EntityState.Dead)
                    entityPlayer.SetDead();
                entityPlayer = new EntityPlayer();
//            SidedConsole.WriteLine("Setting Player Name");
                entityPlayer.nickname = nickName;
//            SidedConsole.WriteLine("Setting Player Pos");
                entityPlayer.Position = new RestrictedVector3D(world.width * 8, world.height * 8);
//            SidedConsole.WriteLine("Setting Player Data");
                entityPlayer.data = Proxy.GetServer().GetDataFor(entityPlayer, password);
                Proxy.GetServer().GetWorldById(0).SpawnEntity(entityPlayer);
            }
        }

        public override void OnServiceUpdate()
        {
            foreach (var bind in KeyBinds.binds)
                if (KeyboardHandler.PressedKeys.ContainsKey(bind.Key) && (bool) KeyboardHandler.PressedKeys[bind.Key])
                    KeyBinds.binds[bind.Key](entityPlayer);
            //bad idea to update it on client side
            entityPlayer.spells.OnUpdate();
        }

        public override void OnExit()
        {
        }
    }
}
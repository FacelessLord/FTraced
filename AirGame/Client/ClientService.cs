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
            player = new Player();
            UpdateRendererData(Proxy.GetServer().GetWorldById(0));
            player.nickname = nickName;
            player.Position = new RestrictedVector3D(world.width * 8, world.height * 8,0);
            player.data = Proxy.GetServer().GetDataFor(player, password);
            
            var testEntity = new Entity(Proxy.GetServer().GetWorldById(0),
                new RestrictedVector3D(world.width * 8, world.height * 8, 0));

            Proxy.GetServer().GetWorldById(0).SpawnEntity(player);
            testEntity.worldObj.SpawnEntity(testEntity);
            
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
        }

        public override void OnExit()
        {
        }
    }
}
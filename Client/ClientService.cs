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

        public string nickName;
        public string password;
        public volatile Player player;

        public ClientService(string _nickName, string _password) : base(Side.Client)
        {
            this.nickName = _nickName;
            this.password = _password;
        }

        public void UpdateRendererData(World _world)
        {
            worldRenderer = new WorldRenderer(_world);
        }

        public override void OnStart()
        {
            player = new Player();
            player.nickname = nickName;
            player.Data = Proxy.GetServer().GetDataFor(nickName, password);
            Proxy.GetServer().GetWorldById(0).SpawnEntity(player);
            UpdateRendererData(Proxy.GetServer().GetWorldById(0));
            KeyBinds.Register();
            GraphicWindow.client = this;
            GraphicWindow.RunWindow();
        }

        public override void OnServiceUpdate()
        {
            foreach (var bind in KeyBinds.binds)
            {
                if(KeyboardHandler.PressedKeys.ContainsKey(bind.Key) && (bool) KeyboardHandler.PressedKeys[bind.Key])
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
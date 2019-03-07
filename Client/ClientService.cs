using System.Net;
using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Utils;

namespace GlLib.Client
{
    public class ClientService : SideService
    {
        private World _currentWorld;
        public string nickName;
        public string password;
        public volatile Player player;

        public ClientService(string nickName, string password) : base(Side.Client)
        {
            this.nickName = nickName;
            this.password = password;
            player = new Player();
        }

        public bool IsConnectedToServer => serverId > -1;

        public World CurrentWorld
        {
            get { return _currentWorld; }
            set
            {
                _currentWorld = value;
                player.worldObj = value;
            }
        }

        public override void OnStart()
        {
            ConnectToIntegratedServer();
            GraphicWindow.RunWindow();
        }

        public override void OnServiceUpdate()
        {
//            CurrentWorld?.Update();
        }

        public override void OnExit()
        {
        }

        public bool ConnectToServer(IPAddress ip)
        {
            //todo
            return false;
        }

        public bool ConnectToIntegratedServer()
        {
            SidedConsole.WriteLine("Connect request");
            var connectRequest = new IntegratedConnectionRequestPacket(this);
            Proxy.SendPacketToServer(connectRequest);
            Proxy.AwaitWhile(() => !IsConnectedToServer);
            SidedConsole.WriteLine("Connected");

            LoadPlayerFromServer();
            var mapRequest = new WorldMapRequest(this, player.Data.worldId);
            Proxy.SendPacketToServer(mapRequest);
            Proxy.AwaitWhile(() => CurrentWorld == null);
            
            CurrentWorld.SpawnEntity(player);
            CurrentWorld.players.TryAdd(player.nickname, player);
            CurrentWorld.LoadWorld();
            return Config.isIntegratedServer;
        }

        public void LoadPlayerFromServer()
        {
            // Getting player data from server
            SidedConsole.WriteLine("Player data request");
            var playerDataRequest = new PlayerDataRequestPacket(this);
            Proxy.SendPacketToServer(playerDataRequest);
            Proxy.AwaitWhile(() => player.Data == null); //waiting for data to be received

            SidedConsole.WriteLine("Client setup");
        }
    }
}
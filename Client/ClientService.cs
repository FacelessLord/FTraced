using System.Net;
using System.Threading;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Common.Registries;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Client
{
    public class ClientService : SideService
    {
        public string _nickName;
        public string _password;
        public volatile Player _player;

        public bool IsConnectedToServer => _serverId > -1;

        public World _currentWorld;

        public ClientService(string nickName, string password) : base(Side.Client)
        {
            _nickName = nickName;
            _password = password;
            _player = new Player();
        }

        public override void OnStart()
        {
        }

        public override void OnServiceUpdate()
        {
            if(_currentWorld != null)
                _currentWorld.Update();
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
            //todo receiving world file
//            _currentWorld = ServerInstance.GetWorldById(0);

            SidedConsole.WriteLine("Connect request");
            IntegratedConnectionRequestPacket connectRequest = new IntegratedConnectionRequestPacket(this);
            Proxy.SendPacketToServer(connectRequest);
            Proxy.AwaitWhile(() =>
            {
                return !IsConnectedToServer;
            });
            SidedConsole.WriteLine("Connected");

            return Config._isIntegratedServer;
        }

        public void LoadPlayerFromServer()
        {
            
            // Getting player data from server
            SidedConsole.WriteLine("Player data request");
            PlayerDataRequestPacket playerDataRequest = new PlayerDataRequestPacket(_nickName, _password);
            Proxy.SendPacketToServer(playerDataRequest);
            Proxy.AwaitWhile(() => _player.Data == null); //waiting for data to be received

            SidedConsole.WriteLine("Client setup");
            _player._worldObj = _player.Data._world;
            _currentWorld = _player.Data._world;
            _player._position = _player.Data._position;
            _player._nickname = _player.Data._nickname;
                
            _currentWorld.SpawnEntity(_player);
            _currentWorld._players.TryAdd(_player._nickname,_player);
        }
    }
}
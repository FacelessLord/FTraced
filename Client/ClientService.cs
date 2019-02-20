using System.Net;
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
    public class ClientService
    {
        public static ClientService _instance;
        public IPAddress _ip;
        public string _nickName;
        public string _password;
        public volatile Player _player;
        
        public ClientPacketHandler _packetHandler;

        public int _serverId = -1;
        public bool IsConnectedToServer => _serverId > -1;

        public State _state = State.Off;

        public World _currentWorld;

        public ClientService(string nickName, string password)
        {
            _nickName = nickName;
            _password = password;
            _packetHandler = new ClientPacketHandler(this);
        }

        public void StartClient()
        {
            _state = State.Starting;
            _player = new Player();
            if (!Config._isIntegratedServer)
            {
                Blocks.Register();
                Entities.Register();
                KeyBinds.Register();
            }
            else
            {
                SidedConsole.WriteLine("Connecting To Integrated Server");
                ConnectToIntegratedServer();
                SidedConsole.WriteLine($"Connection established. ServerId is {_serverId}");
            }
            _packetHandler.StartPacketHandler();

            GraphicWindow.RunWindow();
        }

        public void GameLoop()
        {
            _state = State.Loop;
            while (true)
            {
                _currentWorld.Update();
                Proxy.Sync();
            }
        }

        public void ExitGame()
        {
            _state = State.Exiting;
            //todo
            _state = State.Off;
        }

        public void SetupIp(IPAddress ip)
        {
            _ip = ip;
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

            //todo send connectionPackage and receive ConnectedPackage
            SidedConsole.WriteLine("Connect request");
            ConnectRequestPacket connectRequest = new ConnectRequestPacket(_nickName, _password);
            Proxy.SendPacketToServer(connectRequest);
            Proxy.AwaitWhile(() =>
            {
                return !IsConnectedToServer;
            });
            SidedConsole.WriteLine("Connected");

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
            _currentWorld._players.Add(_player);
            return Config._isIntegratedServer;
        }

        public void HandlePacket(Packet packet)
        {
            SidedConsole.WriteLine($"Packet {packet._packetId} has been received.");
            packet.OnClientReceive(this);
        }
    }
}
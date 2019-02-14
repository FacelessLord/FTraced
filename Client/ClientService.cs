using System.Net;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Common.Registries;
using GlLib.Server;

namespace GlLib.Client
{
    public class ClientService
    {
        public static ClientService _instance;
        public IPAddress _ip;
        public string _nickName;
        public string _password;
        public Player _player;

        public State _state = State.Off;

        public World _currentWorld;

        public ClientService(string nickName, string password)
        {
            _nickName = nickName;
            _password = password;
        }

        public void StartClient()
        {
            _state = State.Starting;
            if (!Config._isIntegratedServer)
            {
                Blocks.Register();
                KeyBinds.Register();
                Entities.Register();
            }

            GraphicCore.Run();
        }

        public void GameLoop()
        {
            _state = State.Loop;
            _currentWorld.Update();
            Proxy.Sync();
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
            _currentWorld = ServerInstance.GetWorldById(0);
            
            // Getting player data from server
            PlayerDataRequestPacket playerDataRequest = new PlayerDataRequestPacket(this._nickName);
            Proxy.SendPacketToServer(playerDataRequest);
            _player = new Player(null,null);
            while(_player._playerData == null)//waiting for data to be received
            {}
            
            return Config._isIntegratedServer;
        }

        public void HandlePackage(Packet packet)
        {
            if (Proxy._awaitedPacketIds.Contains(packet._packetId))
            {
                Proxy._awaitedPacket = packet;
            }
            else
                packet.OnClientReceive(this);
        }
    }
}
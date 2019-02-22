using System;
using System.Collections.Generic;
using System.Threading;
using GlLib.Client;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Common.Registries;
using GlLib.Utils;

namespace GlLib.Server
{
    public class ServerInstance: SideService
    {
        public List<ClientService> _clients = new List<ClientService>();


        public override void OnStart()
        {
            _serverId = new Random().Next();
            CreateWorlds();
        }

        public override void OnServiceUpdate()
        {
            Proxy.Sync();
            foreach (var world in _worlds)
            {
                world.Value.Update();
            }
        }
        
        public override void OnExit()
        {
        }

        public void ConnectClient(ClientService client)
        {
            _clients.Add(client);
            client._player = new Player {Data = GetDataFor(client._nickName, client._password)};
            client._player._worldObj.SpawnEntity(client._player);
            client._player._worldObj.LoadWorld();

            //todo something
        }

        public void CreateWorlds()
        {
            _worlds.Add(0, new World("testmap1.json", 0));
        }

        public Dictionary<int, World> _worlds = new Dictionary<int, World>();

        public Dictionary<string, PlayerData> _playerInfo = new Dictionary<string, PlayerData>();

        public PlayerData GetDataFor(string playerName, string password)
        {
            //todo use password
            if (_playerInfo.ContainsKey(playerName))
                return _playerInfo[playerName];
            World spawnWorld = GetWorldById(0);
            PlayerData data = new PlayerData(spawnWorld,
                new RestrictedVector3D(spawnWorld._width * 8, spawnWorld._height * 8, 0), playerName);
            _playerInfo.Add(playerName, data);
            return data;
        }

        public World GetWorldById(int id)
        {
            return _worlds[id];
        }

        public ServerInstance() : base(Side.Server)
        {
        }
    }
}
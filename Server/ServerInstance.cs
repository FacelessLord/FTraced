using System;
using System.Collections.Generic;
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
    public static class ServerInstance
    {
        public static State _state = State.Off;
        public static List<ClientService> _clients = new List<ClientService>();
        public static int _serverId;

        public static ServerPacketHandler _packetHandler = new ServerPacketHandler();
        
        public static void StartServer()
        {
            _serverId = new Random().Next();
            _state = State.Starting;
//            Console.WriteLine($"Hello World! {1/16}");
            Blocks.Register();
            KeyBinds.Register();
            Entities.Register();
            CreateWorlds();
            _packetHandler.StartPacketHandler();
        }


        public static void GameLoop()
        {
            _state = State.Loop;
            foreach (var world in _worlds)
            {
                world.Value.Update();
            }
        }

        public static void ConnectClient(ClientService client)
        {
            _clients.Add(client);
            client._player = new Player {Data = GetDataFor(client._nickName, client._password)};
            client._player._worldObj.SpawnEntity(client._player);
            client._player._worldObj.LoadWorld();

            //todo something
        }

        public static void ExitGame()
        {
            _state = State.Exiting;
            //todo
            _state = State.Off;
        }

        public static void CreateWorlds()
        {
            _worlds.Add(0, new World("testmap1.json", 0));
        }

        public static Dictionary<int, World> _worlds = new Dictionary<int, World>();

        public static Dictionary<string, PlayerData> _playerInfo = new Dictionary<string, PlayerData>();

        public static PlayerData GetDataFor(string playerName, string password)
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

        public static World GetWorldById(int id)
        {
            return _worlds[id];
        }

        public static void HandlePacket(Packet packet)
        {
            SidedConsole.WriteLine($"Packet {packet._packetId} has been received.");
            packet.OnServerReceive();
        }
    }
}
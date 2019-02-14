using System;
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Common.Registries;
using GlLib.Utils;

namespace GlLib.Server
{
    public static class ServerInstance
    {
        public static State _state = State.Off; 
        
        public static void StartServer()
        {
            _state = State.Starting;
//            Console.WriteLine($"Hello World! {1/16}");
            Blocks.Register();
            KeyBinds.Register();
            Entities.Register();
            CreateWorlds();
        }
        

        public static void GameLoop()
        {
            _state = State.Loop;
            foreach (var world in _worlds)
            {
                world.Value.Update();
            }
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

        public static PlayerData GetDataFor(string playerName)
        {
            if (_playerInfo.ContainsKey(playerName))
                return _playerInfo[playerName];
            World spawnWorld = GetWorldById(0);
            PlayerData data = new PlayerData(spawnWorld,
                new RestrictedVector3D(spawnWorld._width * 8, spawnWorld._height * 8, 0),playerName);

            return data;
        }

        public static World GetWorldById(int id)
        {
            return _worlds[id];
        }

        public static void HandlePackage(Packet packet)
        {
            packet.OnServerReceive();
        }
    }
}
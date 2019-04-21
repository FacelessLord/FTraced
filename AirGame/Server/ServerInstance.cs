using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Json;
using GlLib.Client;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Server
{
    public class ServerInstance : SideService
    {
        public List<ClientService> clients = new List<ClientService>();

        public Dictionary<string, PlayerData> playerInfo = new Dictionary<string, PlayerData>();

        public Dictionary<int, string> registeredWorlds = new Dictionary<int, string>();

        public Dictionary<int, ServerWorld> worlds = new Dictionary<int, ServerWorld>();

        public ServerInstance() : base(Side.Server)
        {
            Proxy.serverInstance = this;
        }


        public override void OnStart()
        {
            serverId = new Random().Next();
            UpdateServerConfiguration();

            CreateWorlds();
            LoadWorlds();
        }

        public override void OnServiceUpdate()
        {
            foreach (var world in worlds) world.Value.Update();
        }

        public override void OnExit()
        {
        }

        public void ConnectClient(ClientService _client)
        {
            clients.Add(_client);
            _client.player = new Player {Data = GetDataFor(_client.nickName, _client.password)};
            var world = GetWorldById(_client.player.Data.worldId);
            world.SpawnEntity(_client.player);
        }

        public void RegisterWorld(int _id, string _worldName)
        {
            registeredWorlds.Add(_id, _worldName);
        }

        public void CreateWorlds()
        {
            foreach (var world in registeredWorlds) worlds.Add(0, new ServerWorld(world.Value, world.Key));
        }

        public void LoadWorlds()
        {
            foreach (var world in worlds.Values)
            {
                var worldJson = File.ReadAllText("maps/" + world.mapName + ".json");
                var parser = new JsonTextParser();
                var obj = parser.Parse(worldJson);
                var mainCollection = (JsonObjectCollection) obj;
                WorldManager.LoadWorld(world, mainCollection);
                world.LoadWorld();
            }
        }

        public PlayerData GetDataFor(string _playerName, string _password)
        {
            //todo use password
            if (playerInfo.ContainsKey(_playerName))
                return playerInfo[_playerName];
            var spawnWorld = GetWorldById(0);
            var data = new PlayerData(spawnWorld.worldId,
                new RestrictedVector3D(spawnWorld.width * 8, spawnWorld.height * 8, 0), _playerName);
            playerInfo.Add(_playerName, data);
            return data;
        }

        public void UpdateServerConfiguration()
        {
            if (!Directory.Exists("server")) Directory.CreateDirectory("server");

            if (!File.Exists("server/worlds.json"))
            {
                File.Create("server/worlds.json").Close();
                File.WriteAllText("server/worlds.json", "0 NewWorld");
                RegisterWorld(0, "server/NewWorld");
            }
            else
            {
                var text = File.ReadLines("server/worlds.json");
                foreach (var line in text)
                {
                    var firstSpaceIndex = line.IndexOf(" ", StringComparison.Ordinal);
                    var number = line.Substring(0, firstSpaceIndex);
                    var name = line.Substring(firstSpaceIndex + 1);
                    RegisterWorld(int.Parse(number), name);
                }
            }
        }

        public ServerWorld GetWorldById(int _id)
        {
            return worlds[_id];
        }

        public string GetWorldName(int _id)
        {
            return registeredWorlds[_id];
        }
    }
}
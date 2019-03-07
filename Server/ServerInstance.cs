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

        public Dictionary<int, World> worlds = new Dictionary<int, World>();

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
            Proxy.Sync();
            foreach (var world in worlds) world.Value.Update();
        }

        public override void OnExit()
        {
        }

        public void ConnectClient(ClientService client)
        {
            clients.Add(client);
            client.player = new Player {Data = GetDataFor(client.nickName, client.password)};

            //todo something
        }

        public void RegisterWorld(int id, string worldName)
        {
            registeredWorlds.Add(id, worldName);
        }

        public void CreateWorlds()
        {
            foreach (var world in registeredWorlds)
            {
                worlds.Add(0, new World(world.Value+".json", world.Key));
            }
        }

        public void LoadWorlds()
        {
            foreach (var world in worlds.Values)
            {
                var worldJson = File.ReadAllText("maps/" + world.mapName);
                var parser = new JsonTextParser();
                var obj = parser.Parse(worldJson);
                var mainCollection = (JsonObjectCollection) obj;
                WorldManager.LoadWorld(world, mainCollection);
                world.LoadWorld();
            }
        }

        public PlayerData GetDataFor(string playerName, string password)
        {
            //todo use password
            if (playerInfo.ContainsKey(playerName))
                return playerInfo[playerName];
            var spawnWorld = GetWorldById(0);
            var data = new PlayerData(spawnWorld.worldId,
                new RestrictedVector3D(spawnWorld.width * 8, spawnWorld.height * 8, 0), playerName);
            playerInfo.Add(playerName, data);
            return data;
        }

        public void UpdateServerConfiguration()
        {
            if (!Directory.Exists("server"))
            {
                Directory.CreateDirectory("server");
            }

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
                    int firstSpaceIndex = line.IndexOf(" ", StringComparison.Ordinal);
                    string number = line.Substring(0, firstSpaceIndex);
                    string name = line.Substring(firstSpaceIndex + 1);
                    RegisterWorld(int.Parse(number), name);
                }
            }
        }

        public World GetWorldById(int id)
        {
            return worlds[id];
        }

        public Dictionary<int, string> registeredWorlds = new Dictionary<int, string>();

        public string GetWorldName(int id)
        {
            return registeredWorlds[id];
        }
    }
}
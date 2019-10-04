using System;
using System.Collections.Generic;
using System.IO;
using GlLib.Client;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;
using GlLib.Utils.Collections;

namespace GlLib.Server
{
    public class ServerInstance : SideService
    {
        public List<ClientService> clients = new List<ClientService>();

        public Dictionary<string, PlayerData> playerInfo = new Dictionary<string, PlayerData>();

        public Dictionary<int, string> registeredWorlds = new Dictionary<int, string>();

        public Dictionary<int, World> worlds = new Dictionary<int, World>();

        public ServerInstance() : base(Side.Server)
        {
        }


        public override void OnStart()
        {
            UpdateServerConfiguration();

            profiler.SetState(State.LoadingWorld);
            CreateWorlds();
            LoadWorlds();
        }

        public override void OnServiceUpdate()
        {
            foreach (var world in worlds)
                world.Value.Update();
            // this code don't work!
            //foreach (var client in clients)
            //    client.player.spells.OnUpdate();
        }

        public override void OnExit()
        {
        }

        public void RegisterWorld(int _id, string _worldName)
        {
            registeredWorlds.Add(_id, _worldName);
        }

        public void CreateWorlds()
        {
            foreach (var world in registeredWorlds) worlds.Add(0, new World(world.Value, world.Key));
        }

        public void LoadWorlds()
        {
            foreach (var world in worlds.Values) WorldManager.LoadWorld(world);
        }

        public PlayerData GetDataFor(Player _player, string _password)
        {
            //todo use password
            if (playerInfo.ContainsKey(_player.nickname))
                return playerInfo[_player.nickname];
            var data = new PlayerData();
            playerInfo.Add(_player.nickname, data);
            return data;
        }

        public void UpdateServerConfiguration()
        {
            if (!Directory.Exists("server")) Directory.CreateDirectory("server");

            if (!File.Exists("server/worlds.json"))
            {
                File.Create("server/worlds.json").Close();
                File.WriteAllText("server/worlds.json", "0 Overworld");
                RegisterWorld(0, "Overworld");
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

        public World GetWorldById(int _id)
        {
            return worlds[_id];
        }

        public string GetWorldName(int _id)
        {
            return registeredWorlds[_id];
        }
    }
}
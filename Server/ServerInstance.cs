using System;
using System.Collections.Generic;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Server
{
    public class ServerInstance
    {
        public void StartServer()
        {
            
        }
        
        public World _spawnWorld = new World("testmap1.json");
        
        public Dictionary<string, PlayerData> _playerInfo = new Dictionary<string, PlayerData>();

        public PlayerData GetDataFor(string playerName)
        {
            if (_playerInfo.ContainsKey(playerName))
                return _playerInfo[playerName];
            PlayerData data = new PlayerData(_spawnWorld,
                new RestrictedVector3D(_spawnWorld._width * 8, _spawnWorld._height * 8, 0));
            
            return data;
        }
    }
}
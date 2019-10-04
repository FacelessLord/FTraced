using System.Net.Json;
using GlLib.Common.Api;
using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils.Collections;

namespace GlLib.Common
{
    public class PlayerData : IJsonSerializable
    {
        
        public PlayerData()
        {
        }

        public ushort CastLevel { get; } = 100;
        public int WorldId { get; } = 0;

        public JsonObject Serialize(string _objectName)
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            throw new System.NotImplementedException();
        }

        public string GetStandardName()
        {
            return "playerdata";
        }

        /// <summary>
        /// Sets stored data to Player fields 
        /// </summary>
        /// <param name="_player">Player to imprint to</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ImprintToPlayer(Player _player)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Creates data based on current Player field values
        /// </summary>
        /// <param name="_player">Player to get data from</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void GetDataFromPlayer(Player _player)
        {
            throw new System.NotImplementedException();
        }
    }
}
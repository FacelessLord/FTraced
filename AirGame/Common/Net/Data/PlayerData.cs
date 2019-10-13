using System;
using System.Net.Json;
using GlLib.Common.Api;
using GlLib.Common.Entities;

namespace GlLib.Common
{
    public class PlayerData : IJsonSerializable
    {
        public ushort CastLevel { get; } = 100;
        public int WorldId { get; } = 0;

        public JsonObject Serialize(string _objectName)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            throw new NotImplementedException();
        }

        public string GetStandardName()
        {
            return "playerdata";
        }

        /// <summary>
        ///     Sets stored data to Player fields
        /// </summary>
        /// <param name="_player">Player to imprint to</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ImprintToPlayer(Player _player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates data based on current Player field values
        /// </summary>
        /// <param name="_player">Player to get data from</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void GetDataFromPlayer(Player _player)
        {
            throw new NotImplementedException();
        }
    }
}
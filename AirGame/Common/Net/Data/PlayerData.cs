using System.Net.Json;
using GlLib.Common.Api;
using GlLib.Common.Map;
using GlLib.Utils.Collections;

namespace GlLib.Common
{
    public class PlayerData : IJsonSerializable
    {
        public World world;
        
        public PlayerData()
        {
            CastLevel = 100;
        }

        public ushort CastLevel { get; }

        public void SaveToNbt(NbtTag _tag)
        {
        }

        public static PlayerData LoadFromNbt(NbtTag _tag)
        {
            return new PlayerData();
        }

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
    }
}
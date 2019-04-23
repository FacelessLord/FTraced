using System.Net.Json;

namespace GlLib.Common.API
{
    public interface IJsonSerializable
    {
        JsonObject CreateJsonObject();
        void LoadFromJsonObject(JsonObject _jsonObject);
    }
}
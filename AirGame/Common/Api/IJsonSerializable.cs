using System.Net.Json;

namespace GlLib.Common.API
{
    public interface IJsonSerializable
    {
        JsonObject CreateJsonObject(string _objectName);
        void LoadFromJsonObject(JsonObject _jsonObject);

        string GetStandardName();
    }
}
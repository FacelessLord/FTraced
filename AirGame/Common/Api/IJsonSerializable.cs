using System.Net.Json;

namespace GlLib.Common.Api
{
    public interface IJsonSerializable
    {
        /// <summary>
        ///     Creates New JsonObject Instance
        /// </summary>
        /// <param name="_objectName"></param>
        /// <returns></returns>
        JsonObject Serialize(string _objectName);

        /// <summary>
        ///     Desearializes JsonObject to current instance
        /// </summary>
        /// <param name="_jsonObject"></param>
        /// <returns></returns>
        void Deserialize(JsonObject _jsonObject);

        /// <summary>
        ///     Name used in internal transitions
        /// </summary>
        /// <returns></returns>
        string GetStandardName();
    }
}
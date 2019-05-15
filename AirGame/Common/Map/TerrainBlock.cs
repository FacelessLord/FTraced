using System.Net.Json;
using GlLib.Client.API;
using GlLib.Common.API;

namespace GlLib.Common.Map
{
    public abstract class TerrainBlock : IJsonSerializable
    {
        public int id = -1;

        /// <summary>
        ///     name in format : [mod:]block.[blockset.]blockname[.subtype]
        /// </summary>
        /// <returns></returns>
        public abstract string Name { get; protected set; }

        public abstract string TextureName { get; internal set; }

        public JsonObject CreateJsonObject()
        {
            var jsonObj = new JsonObjectCollection(GetType().Name);
            jsonObj.Add(new JsonStringValue("Name", Name));
            jsonObj.Add(new JsonStringValue("TextureName", TextureName));

            return jsonObj;
        }

        public void LoadFromJsonObject(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection collection)
            {
                Name = ((JsonStringValue) collection[0]).Value;
                TextureName = ((JsonStringValue) collection[1]).Value;
            }
        }

        public virtual bool RequiresSpecialRenderer(World _world, int _x, int _y)
        {
            return false;
        }

        public virtual IBlockRenderer GetSpecialRenderer(World _world, int _x, int _y)
        {
            return null;
        }
    }
}
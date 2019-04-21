using System.Collections.Generic;
using System.Linq;
using System.Net.Json;
using GlLib.Common.API;

namespace GlLib.Utils
{
    public class JsonHelper
    {
        public static JsonArrayCollection SaveList<T>(string _name, List<T> _objects) where T : IJsonSerializable
        {
            return new JsonArrayCollection(_name, _objects?.Select(_o => _o.CreateJsonObject()));
        }

        public static List<T> LoadList<T>(JsonArrayCollection _objects)
            where T : IJsonSerializable, new()
        {
            return _objects.Select(_o =>
            {
                var obj = new T();
                obj.LoadFromJsonObject(_o);
                return obj;
            }).ToList();
        }
    }
}
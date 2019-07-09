using System.Collections.Generic;
using System.Linq;
using System.Net.Json;
using GlLib.Common.Api;

namespace GlLib.Utils
{
    public class JsonHelper
    {
        public static JsonArrayCollection SaveList<T>(string _name, List<T> _objects) where T : IJsonSerializable
        {
            return new JsonArrayCollection(_name, _objects?.Select(_o => _o.Serialize(_o.GetStandardName())));
        }

        public static List<T> LoadList<T>(JsonArrayCollection _objects)
            where T : IJsonSerializable, new()
        {
            return _objects.Select(_o =>
            {
                var obj = new T();
                obj.Deserialize(_o);
                return obj;
            }).ToList();
        }

        public static JsonArrayCollection SaveDict<T>(string _name, Dictionary<string, T> _objects)
            where T : IJsonSerializable
        {
            return new JsonArrayCollection(_name, _objects.Keys?.Select(_k => _objects[_k].Serialize(_k)));
        }

        public static Dictionary<string, T> LoadDict<T>(JsonArrayCollection _objects)
            where T : IJsonSerializable, new()
        {
            var dict = new Dictionary<string, T>();
            var values = _objects.Select(_o =>
            {
                var obj = new T();
                obj.Deserialize(_o);
                dict.Add(_o.Name, obj);
                return obj;
            }).ToList();
            return dict;
        }
    }
}
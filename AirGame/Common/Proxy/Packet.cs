using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Common.Api;

namespace GlLib.Common
{
    public class Packet : IJsonSerializable
    {
        public PacketType type;
        public int id;

        public Dictionary<string, JsonObject> data = new Dictionary<string, JsonObject>();

        /// <summary>
        /// Empty constructor used to enable IJsonSerializable to work
        /// Don't use it in other way
        /// </summary>
        public Packet()
        {
        }
        public Packet(PacketType _type, int _id = 0)
        {
            type = _type;
            id = _id;
        }
        
        public void Add(string _name, IJsonSerializable _value)
        {
            data.Add(_name, _value.CreateJsonObject(_name));
        }
        
        public T Get<T>(string _key) where T : IJsonSerializable, new()
        {
            T t = new T();
            t.LoadFromJsonObject(data[_key]);
            return t;
        }


        public JsonObject CreateJsonObject(string _objectName)
        {
            var collection = new JsonObjectCollection();
            
            var jid = new JsonNumericValue("id", id);
            var jtype = new JsonStringValue("type", type.ToString());
            var dataColl = new JsonObjectCollection("data", data.Values);

            collection.Add(jtype);
            collection.Add(jid);
            collection.Add(dataColl);

            return collection;
        }

        public void LoadFromJsonObject(JsonObject _jsonObject)
        {
            if(_jsonObject is JsonObjectCollection coll)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    var obj = coll[i];
                    if (obj.Name == "id" && obj is JsonNumericValue jn)
                    {
                        id = (int) jn.Value;
                    }else
                    if (obj.Name == "type" && obj is JsonStringValue js)
                    {
                        Enum.TryParse(js.Value, out type);
                    }
                    else
                    if (obj.Name == "data" && obj is JsonObjectCollection jc)
                    {
                        for (int j = 0; j < jc.Count; j++)
                        {
                            var dataObj = jc[j];
                            data.Add(dataObj.Name, dataObj);
                        }
                    }
                }

                return;
            }
            
            throw new ArgumentException("Packet can only be loaded from JsonCollection");
        }

        public string GetStandardName()
        {
            return type.ToString();
        }
    }

    public enum PacketType
    {
        WorldSynchronization,
        KeyNotification,
        GuiNotification,
        ChatNotification,
        SoundNotification,
        VisualNotification,
        DebugNotification
    }
}
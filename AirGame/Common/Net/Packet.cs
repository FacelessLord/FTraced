using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Common.Api;

namespace GlLib.Common
{
    public class Packet : IJsonSerializable
    {
        /// <summary>
        /// Packet type
        /// </summary>
        public PacketType type;

        /// <summary>
        /// Packet subtype
        /// </summary>
        public int id;

        /// <summary>
        /// Serialized data needed to be sent
        /// </summary>
        public Dictionary<string, JsonObject> data = new Dictionary<string, JsonObject>();

        /// <summary>
        /// Empty constructor used to enable IJsonSerializable to work
        /// Don't use it in other way
        /// </summary>
        public Packet()
        {
        }

        /// <summary>
        /// Initializes new Packet Instance 
        /// </summary>
        /// <param name="_type">Packet type</param>
        /// <param name="_id">Packet subtype</param>
        public Packet(PacketType _type, int _id = 0)
        {
            type = _type;
            id = _id;
        }

        /// <summary>
        /// Adds new data to packet to send
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_value"></param>
        /// <remarks>
        /// Serializes provided data and stores it till the packet will be sent
        /// </remarks>
        public void Add(string _name, IJsonSerializable _value)
        {
            data.Add(_name, _value.Serialize(_name));
        }

        /// <summary>
        /// Adds Serialized JsonObject to packet to send
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_value"></param>
        /// <remarks>
        /// Serializes provided data and stores it till the packet will be sent
        /// </remarks>
        public void AddJson(string _name, JsonObject _value)
        {
            data.Add(_name, _value);
        }

        /// <summary>
        /// Retrieves data from packet
        /// </summary>
        /// <param name="_key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Deserialized data object</returns>
        public T Get<T>(string _key) where T : IJsonSerializable, new()
        {
            T t = new T();
            t.Deserialize(data[_key]);
            return t;
        }

        /// <summary>
        /// Retrieves Serialized JsonObject from packet
        /// </summary>
        /// <param name="_key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Serialized data object(JsonObject)</returns>
        public JsonObject GetJson(string _key)
        {
            return data[_key];
        }


        public JsonObject Serialize(string _objectName)
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

        public void Deserialize(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection coll)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    var obj = coll[i];
                    if (obj.Name == "id" && obj is JsonNumericValue jn)
                    {
                        id = (int) jn.Value;
                    }
                    else if (obj.Name == "type" && obj is JsonStringValue js)
                    {
                        Enum.TryParse(js.Value, out type);
                    }
                    else if (obj.Name == "data" && obj is JsonObjectCollection jc)
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

    /// <summary>
    /// WorldSynchronization is sent every server tick and only from Server to Client
    /// 
    /// Notification packets are sent whenever they needed to be sent both
    /// from Server to Client and from Client to Server
    /// </summary>
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
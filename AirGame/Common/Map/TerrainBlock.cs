using System;
using System.Net.Json;
using GlLib.Client.Api.Renderers;
using GlLib.Common.Api;
using GlLib.Utils.Math;

namespace GlLib.Common.Map
{
    public abstract class TerrainBlock : IJsonSerializable
    {
        protected const string Path = @"blocks/";
        public int id = -1;

        /// <summary>
        ///     name in format : [mod:]block.[blockset.]blockname[.subtype]
        /// </summary>
        /// <returns></returns>
        public abstract string Name { get; protected set; }

        public abstract string TextureName { get; internal set; }
        public Rotation Rotation { get; protected set; }

        public JsonObject Serialize(string _objectName)
        {
            var jsonObj = new JsonObjectCollection(_objectName);
            jsonObj.Add(new JsonStringValue("Name", GetInternalName()));
            jsonObj.Add(new JsonStringValue("TextureName", TextureName));

            return jsonObj;
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection collection)
            {
                var pair = FromInternalString(((JsonStringValue) collection[0]).Value);
                Name = pair.Item1;
                Rotation = (Rotation) pair.Item2;
                TextureName = ((JsonStringValue) collection[1]).Value;
            }
        }

        public string GetStandardName()
        {
            return "block";
        }

        public void SetRotation(int _angle)
        {
            Rotation = (Rotation) _angle;
        }

        public void SetRotation(Rotation _rotation)
        {
            Rotation = _rotation;
        }

        public virtual AxisAlignedBb GetCollisionBox()
        {
            return AxisAlignedBb.Zero;
        }

        public virtual bool RequiresSpecialRenderer(World _world, int _x, int _y)
        {
            return false;
        }

        public string GetInternalName()
        {
            return Name + ":" + Rotation;
        }

        public Tuple<string, int> FromInternalString(string _str)
        {
            var parsed = _str.Split(':');
            if (parsed.Length == 2)
                switch (parsed[1])
                {
                    case "Down":
                        return Tuple.Create(parsed[0], (int) Rotation.Down);
                    case "Left":
                        return Tuple.Create(parsed[0], (int) Rotation.Left);
                    case "Up":
                        return Tuple.Create(parsed[0], (int) Rotation.Up);
                    case "Right":
                        return Tuple.Create(parsed[0], (int) Rotation.Right);
                    default:
                        return Tuple.Create(_str, (int) Rotation.Down);
                }

            return Tuple.Create(_str, (int) Rotation.Down); // return default block 
        }


        public virtual IBlockRenderer GetSpecialRenderer(World _world, int _x, int _y)
        {
            return null;
        }
    }
}
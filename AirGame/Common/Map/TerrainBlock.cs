using System;
using System.Net.Json;
using GlLib.Client.Api.Renderers;
using GlLib.Common.Api;
using GlLib.Utils;
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

        public void SetRotation(int _angle)
        {
            Rotation = (Rotation) _angle;
        }

        public void SetRotation(Rotation _rotation)
        {
            Rotation = _rotation;
        }

        public JsonObject CreateJsonObject(string _objectName)
        {
            var jsonObj = new JsonObjectCollection(_objectName);
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

        public string GetStandardName()
        {
            return "block";
        }

        public virtual AxisAlignedBb GetCollisionBox()
        {
            return AxisAlignedBb.Zero;
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

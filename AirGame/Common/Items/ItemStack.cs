using System.Collections.Generic;
using System.Net.Json;
using GlLib.Common.Api;
using GlLib.Utils;
using GlLib.Utils.Collections;

namespace GlLib.Common.Items
{
    public class ItemStack : IJsonSerializable
    {
        /// <summary>
        /// Item held in this stack
        /// </summary>
        public Item item;
        
        /// <summary>
        /// Items count
        /// </summary>
        public int stackSize;
        
        /// <summary>
        /// Tag that contains this stack data
        /// </summary>
        public NbtTag stackTag;
        
        public ItemStack(Item _item = null, int _stackSize = 1, NbtTag _tag = null)
        {
            (item, stackSize, stackTag) = (_item, _stackSize, _tag);
        }

        public JsonObject Serialize(string _objectName)
        {
            var coll = new JsonArrayCollection(_objectName);
            coll.Add(new JsonNumericValue("id", item.id));
            coll.Add(new JsonNumericValue("size", stackSize));
            coll.Add(new JsonStringValue("tag", stackTag.ToString()));

            return coll;
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonArrayCollection jsonCollection)
            {
                var itemId = (int) ((JsonNumericValue) jsonCollection[0]).Value;
                item = Proxy.GetClient().registry.GetItemFromId(itemId);
                stackSize = (int) ((JsonNumericValue) jsonCollection[1]).Value;
                stackTag = NbtTag.FromString(((JsonStringValue) jsonCollection[2]).Value);
            }
        }

        public string GetStandardName()
        {
            return "itemStack";
        }

        /// <summary>
        /// Name used in tooltip
        /// </summary>
        /// <returns></returns>
        private string GetName()
        {
            return item.GetName(this);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = item != null ? item.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ stackSize.GetHashCode();
                hashCode = (hashCode * 397) ^ (stackTag != null ? stackTag.GetHashCode() : 0);
                return hashCode;
            }
        }

        public List<string> GetTooltip()
        {
            var tooltip = new List<string>();
            
            tooltip.Add(GetName());
            item.AddInformation(this, tooltip);
            return tooltip;
        }
    }
}
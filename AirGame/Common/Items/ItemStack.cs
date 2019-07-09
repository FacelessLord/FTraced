using System.Net.Json;
using GlLib.Common.Api;
using GlLib.Utils;
using GlLib.Utils.Collections;

namespace GlLib.Common.Items
{
    public class ItemStack : IJsonSerializable
    {
        public Item item;
        public int stackSize;
        public NbtTag stackTag;

        public ItemStack(Item _item, int _stackSize = 1, NbtTag _tag = null)
        {
            (item, stackSize, stackTag) = (_item, _stackSize, _tag);
        }

        public JsonObject Serialize(string _objectName)
        {
            var tag = new NbtTag();
            SaveToNbt(tag);
            return new JsonStringValue(_objectName, tag.ToString());
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonStringValue jsonString)
            {
                var jsonStack = LoadFromJson(jsonString);
                item = jsonStack.item;
                stackSize = jsonStack.stackSize;
                stackTag = jsonStack.stackTag;
            }
        }

        public string GetStandardName()
        {
            return "itemStack";
        }

        public static ItemStack LoadFromJson(JsonStringValue _rawTag)
        {
            var itemTag = NbtTag.FromString(_rawTag.Value);
            var item = Proxy.GetRegistry().GetItemFromId(itemTag.GetInt("ItemId"));
            return new ItemStack(item);
        }

        public virtual void SaveToNbt(NbtTag _tag)
        {
            _tag.Set("ItemStack", GetName());
            if (item != null)
            {
                _tag.Set("Item", item + "");
                _tag.Set("StackSize", stackSize);

                if (stackTag != null)
                    _tag.AppendTag(stackTag, "ItemTag");
            }
        }

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
    }
}
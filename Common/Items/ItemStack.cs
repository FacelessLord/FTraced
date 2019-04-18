using System.Net.Json;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Items
{
    public class ItemStack
    {
        public Item item;
        public int stackSize;
        public NbtTag tag;
        public string unlocalizedName = "stackItem.null";

        public ItemStack(Item _item, int _stackSize = 1, NbtTag _tag = null)
        {
            (item, stackSize, tag) = (_item, _stackSize, _tag);
        }

        public static ItemStack LoadFromJson(JsonStringValue _rawTag, Chunk _chunk)
        {
            var itemTag = NbtTag.FromString(_rawTag.Value);
            var item = Proxy.GetRegistry().GetItemFromId(itemTag.GetInt("ItemId"));
            return new ItemStack(item);
        }

        public virtual void SaveToNbt(NbtTag _tag)
        {
            _tag.SetString("ItemStack", GetName());
            if (item != null)
            {
                _tag.SetString("Item", item + "");
                _tag.SetInt("StackSize", stackSize);

                if (tag != null)
                    _tag.AppendTag(tag, "ItemTag");
            }
        }

        private string GetName()
        {
            return unlocalizedName;
        }

        public JsonStringValue CreateJsonObj()
        {
            var tag = new NbtTag();
            SaveToNbt(tag);
            return new JsonStringValue("Item" + GetHashCode(), tag.ToString());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = item != null ? item.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ stackSize.GetHashCode();
                hashCode = (hashCode * 397) ^ (tag != null ? tag.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
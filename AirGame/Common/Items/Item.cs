using GlLib.Client.API;

namespace GlLib.Common.Items
{
    public class Item
    {
        public string unlocalizedName = "item.null";

        public virtual string GetName(ItemStack _itemStack)
        {
            return unlocalizedName;
        }

        public string GetTextureName(ItemStack _itemStack)
        {
            return unlocalizedName + ".png";
        }

        public virtual bool RequiresSpecialRenderer(ItemStack _itemStack)
        {
            return false; //todo
        }

        public virtual IItemRenderer GetSpecialRenderer(ItemStack _itemStack)
        {
            return null; //todo
        }
    }
}
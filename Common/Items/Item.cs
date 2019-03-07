using GlLib.Client.API;

namespace GlLib.Common.Items
{
    public class Item
    {
        public string unlocalizedName = "item.null";

        public virtual string GetName(ItemStack itemStack)
        {
            return unlocalizedName;
        }

        public string GetTextureName(ItemStack itemStack)
        {
            return unlocalizedName + ".png";
        }

        public virtual bool RequiresSpecialRenderer(ItemStack itemStack)
        {
            return false; //todo
        }

        public virtual IItemRenderer GetSpecialRenderer(ItemStack itemStack)
        {
            return null; //todo
        }
    }
}
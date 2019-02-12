using GlLib.Client.API;
using GlLib.Common.Map;

namespace GlLib.Common.Items
{
    public class Item
    {
        public string _unlocalizedName = "item.null";

        public virtual string GetName(ItemStack itemStack)
        {
            return _unlocalizedName;
        }

        public string GetTextureName(ItemStack itemStack)
        {
            return _unlocalizedName + ".png";
        }
        
        public virtual bool RequiresSpecialRenderer(ItemStack itemStack)
        {
            return false;//todo
        }

        public virtual IItemRenderer GetSpecialRenderer(ItemStack itemStack)
        {
            return null;//todo
        }
    }
}
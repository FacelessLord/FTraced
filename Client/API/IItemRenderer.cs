using GlLib.Common.Items;
using GlLib.Common.Map;

namespace GlLib.Client.API
{
    public interface IItemRenderer
    {
        void Render(ItemStack itemStack,ItemRenderType type);
    }

    public enum ItemRenderType
    {
        Inventory,Equipped,Dropped
    }
}
using GlLib.Common.Items;

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
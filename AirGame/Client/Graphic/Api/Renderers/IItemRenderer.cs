using GlLib.Common.Items;

namespace GlLib.Client.Api.Renderers
{
    public interface IItemRenderer
    {
        void Render(ItemStack _itemStack, ItemRenderType _type);
    }

    public enum ItemRenderType
    {
        Inventory,
        Equipped,
        Dropped
    }
}
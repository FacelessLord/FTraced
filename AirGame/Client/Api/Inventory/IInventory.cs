using GlLib.Common.Items;

namespace GlLib.Client.API.Inventory
{
    public interface IInventory
    {
        int GetMaxSize();
        int GetCurrentSize();
        void AddItemStack(ItemStack _itemStack);
        void RemoveItemStack(int _slot);
        string GetInventoryName();
        ItemStack GetStackInSlot(int _slot);
    }
}
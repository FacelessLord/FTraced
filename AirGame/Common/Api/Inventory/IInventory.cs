using GlLib.Common.Items;

namespace GlLib.Common.Api.Inventory
{
    public interface IInventory
    {
        int GetMaxSize();
        int GetCurrentSize();
        int GetSelectedSlot();
        void SelectSlot(int _slotId);
        void AddItemStack(ItemStack _itemStack);
        void SetItemStack(ItemStack _itemStack, int _slot);
        void RemoveItemStack(int _slot);
        string GetInventoryName();
        ItemStack GetStackInSlot(int _slot);

        bool IsItemValidForSlot(ItemStack _item, int _slot);
    }
}
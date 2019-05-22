using GlLib.Common.Items;

namespace GlLib.Common.Api.Inventory
{
    public class Slot : IInventory
    {
        public IInventory inventory;
        public int slotId;

        public Slot(IInventory _inventory, int _slotId)
        {
            inventory = _inventory;
            slotId = _slotId;
        }

        public ItemStack GetStack()
        {
            return inventory.GetStackInSlot(slotId);
        }

        public void SetStack(ItemStack _stack)
        {
            inventory.SetItemStack(_stack, slotId);
        }

        public int GetMaxSize()
        {
            return inventory.GetMaxSize();
        }

        public int GetCurrentSize()
        {
            return inventory.GetCurrentSize();
        }

        public int GetSelectedSlot()
        {
            return inventory.GetSelectedSlot();
        }

        public void SelectSlot(int _slotId)
        {
            inventory.SelectSlot(_slotId);
        }

        public void AddItemStack(ItemStack _itemStack)
        {
            inventory.AddItemStack(_itemStack);
        }

        public void SetItemStack(ItemStack _itemStack, int _slot = -1)
        {
            if (_slot == -1)
                inventory.SetItemStack(_itemStack, slotId);
            else

                inventory.SetItemStack(_itemStack, _slot);
        }

        public void RemoveItemStack(int _slot = -1)
        {
            if (_slot == -1)
                inventory.RemoveItemStack(slotId);
            else

                inventory.RemoveItemStack(_slot);
        }

        public string GetInventoryName()
        {
            return inventory.GetInventoryName();
        }

        public ItemStack GetStackInSlot(int _slot)
        {
            return inventory.GetStackInSlot(_slot);
        }

        public bool IsItemValidForSlot(ItemStack _item, int _slot)
        {
            return inventory.IsItemValidForSlot(_item, _slot);
        }
    }
}
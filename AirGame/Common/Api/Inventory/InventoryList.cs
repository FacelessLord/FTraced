using System.Collections.Generic;
using GlLib.Common.Items;

namespace GlLib.Common.Api.Inventory
{
    public abstract class InventoryList : IInventory
    {
        public List<ItemStack> itemList = new List<ItemStack>();

        public int selectedSlot = -1;

        public abstract int GetMaxSize();

        public int GetCurrentSize()
        {
            return itemList.Count;
        }

        public int GetSelectedSlot()
        {
            return selectedSlot;
        }

        public void SelectSlot(int _slotId)
        {
            selectedSlot = _slotId;
        }

        public abstract string GetInventoryName();

        public ItemStack GetStackInSlot(int _slot)
        {
            if (_slot < GetCurrentSize())
                return itemList[_slot];
            return null;
        }

        public void AddItemStack(ItemStack _itemStack)
        {
            itemList.Add(_itemStack);
        }

        public void SetItemStack(ItemStack _itemStack, int _slot)
        {
            itemList[_slot] = _itemStack;
        }

        public void RemoveItemStack(int _slot)
        {
            itemList.RemoveAt(_slot);
        }
    }
}
using System.Collections.Generic;
using GlLib.Common.Items;

namespace GlLib.Client.API.Inventory
{
    public abstract class InventoryList : IInventory
    {
        public List<ItemStack> itemList = new List<ItemStack>();

        public abstract int GetMaxSize();

        public int GetCurrentSize()
        {
            return itemList.Count;
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

        public void RemoveItemStack(int _slot)
        {
            itemList.RemoveAt(_slot);
        }
    }
}
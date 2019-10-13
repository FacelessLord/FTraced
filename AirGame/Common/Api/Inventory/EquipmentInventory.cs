using System;
using GlLib.Common.Items;
using static GlLib.Common.Items.ItemType;

namespace GlLib.Common.Api.Inventory
{
    public class EquipmentInventory : IInventory
    {
        public ItemStack[] inventory;
        public int selectedSlot;

        public EquipmentInventory()
        {
            inventory = new ItemStack[GetMaxSize()];
        }

        public int GetMaxSize()
        {
            return 9;
        }

        public int GetCurrentSize()
        {
            return 9;
        }

        public int GetSelectedSlot()
        {
            return selectedSlot;
        }

        public void SelectSlot(int _slotId)
        {
            selectedSlot = _slotId;
        }

        public void AddItemStack(ItemStack _itemStack)
        {
            throw new InvalidOperationException();
        }

        public void SetItemStack(ItemStack _itemStack, int _slot)
        {
            inventory[_slot] = _itemStack;
        }

        public void RemoveItemStack(int _slot)
        {
            inventory[_slot] = null;
        }

        public string GetInventoryName()
        {
            return "equipment.player";
        }

        public ItemStack GetStackInSlot(int _slot)
        {
            return inventory[_slot];
        }

        public bool IsItemValidForSlot(ItemStack _item, int _slot)
        {
            if (_item is null) return true;
            var type = _item.item.type;
            switch (_slot)
            {
                case 0:
                case 1:
                    return type is Weapon || type is Shield;
                case 2:
                    return type is Helmet;
                case 3:
                    return type is Armor;
                case 4:
                    return type is Belt;
                case 5:
                    return type is Boots;
                case 6:
                case 7:
                    return type is Ring;
                case 8:
                    return type is Varia;
                default: return false;
            }
        }
    }
}
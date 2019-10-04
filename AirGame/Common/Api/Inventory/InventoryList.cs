using System.Collections.Generic;
using System.Linq;
using System.Net.Json;
using GlLib.Common.Items;
using GlLib.Utils;

namespace GlLib.Common.Api.Inventory
{
    public abstract class InventoryList : IInventory, IJsonSerializable
    {
        /// <summary>
        /// List of items stored in Inventory
        /// </summary>
        public List<ItemStack> itemList = new List<ItemStack>();

        /// <summary>
        /// Currently selected slot
        /// </summary>
        private int selectedSlot;
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

        public bool IsItemValidForSlot(ItemStack _item, int _slot)
        {
            return true;
        }

        public void AddItemStack(ItemStack _itemStack)
        {
            if ((_itemStack is null && !EnableNull) || itemList.Contains(_itemStack))
            {
                return;
            }

            itemList.Add(_itemStack);
        }

        private bool enableNull = false;

        public bool EnableNull
        {
            get => enableNull;
            set
            {
                if (!value)
                    itemList = itemList.Where(_o => !(_o is null)).ToList();
                enableNull = value;
            }
        }

        public void SetItemStack(ItemStack _itemStack, int _slot)
        {
            AddItemStack(_itemStack);
        }

        public void RemoveItemStack(int _slot)
        {
            if (_slot < itemList.Count)
            {
                if (EnableNull)
                    itemList[_slot] = null;
                else
                    itemList.RemoveAt(_slot);
            }
        }

        public JsonObject Serialize(string _objectName)
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            throw new System.NotImplementedException();
        }

        public string GetStandardName()
        {
            throw new System.NotImplementedException();
        }
    }
}
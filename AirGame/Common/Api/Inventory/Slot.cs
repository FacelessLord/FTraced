using GlLib.Common.Items;

namespace GlLib.Common.Api.Inventory
{
    public class Slot
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
    }
}
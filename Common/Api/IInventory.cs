using GlLib.Common.Items;
using GlLib.Utils;

namespace GlLib.Common.API
{
    public interface IInventory
    {
        ItemStack GetStackInSlot(int slot);
        ItemStack SetInventoryContents(int slot, ItemStack itemStack);
        void SaveInventoryTo(NbtTag tag);
        void LoadInventoryFrom(NbtTag tag);
    }
}
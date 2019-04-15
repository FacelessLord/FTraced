using GlLib.Common.Items;
using GlLib.Utils;

namespace GlLib.Common.API
{
    public interface IInventory
    {
        ItemStack GetStackInSlot(int _slot);
        ItemStack SetInventoryContents(int _slot, ItemStack _itemStack);
        void SaveInventoryTo(NbtTag _tag);
        void LoadInventoryFrom(NbtTag _tag);
    }
}
using GlLib.Utils;

namespace GlLib.Common.Items
{
    public class ItemStack
    {
        public Item item;
        public int stackSize;
        public NbtTag tag;

        public ItemStack(Item item, int stackSize = 1, NbtTag tag = null)
        {
            (this.item, this.stackSize, this.tag) = (item, stackSize, tag);
        }
    }
}
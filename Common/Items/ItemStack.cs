using GlLib.Utils;

namespace GlLib.Common.Items
{
    public class ItemStack
    {
        public Item _item;
        public int _stackSize;
        public NbtTag _tag;

        public ItemStack(Item item, int stackSize = 1, NbtTag tag = null)
        {
            (_item, _stackSize, _tag) = (item, stackSize, tag);
        }
    }
}
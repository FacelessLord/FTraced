using GlLib.Utils;

namespace GlLib.Common.Items
{
    public class ItemStack
    {
        public Item item;
        public int stackSize;
        public NbtTag tag;

        public ItemStack(Item _item, int _stackSize = 1, NbtTag _tag = null)
        {
            (item, stackSize, tag) = (_item, _stackSize, _tag);
        }
    }
}
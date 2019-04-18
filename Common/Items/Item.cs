using GlLib.Client.API;

namespace GlLib.Common.Items
{
    public class Item
    {
        public string unlocalizedName = "item.null";

        public Item(int _id, string _name)
        {
            id = _id;
            name = _name;
        }

        public string name { get; set; }
        public int id { get; set; }

        public virtual string GetName(ItemStack _itemStack)
        {
            return unlocalizedName;
        }


        public string GetTextureName(ItemStack _itemStack)
        {
            return unlocalizedName + ".png";
        }

        public virtual bool RequiresSpecialRenderer(ItemStack _itemStack)
        {
            return false; //todo
        }

        public virtual IItemRenderer GetSpecialRenderer(ItemStack _itemStack)
        {
            return null; //todo
        }

        public override string ToString()
        {
            return ""; // todo
        }
    }
}
using GlLib.Client.API;

namespace GlLib.Common.Items
{
    public class Item
    {
        public int id;

        public string name;
        public ItemRarity rarity;

        public string sprite;
        public ItemType type;
        public string unlocalizedName = "item.null";

        public Item(int _id, string _name, string _sprite, ItemRarity _rarity, ItemType _type = ItemType.Varia)
        {
            rarity = _rarity;
            id = _id;
            sprite = _sprite;
            name = _name;
            type = _type;
        }

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
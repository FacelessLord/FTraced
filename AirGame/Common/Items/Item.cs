using GlLib.Client.Api.Sprites;
using GlLib.Client.API;

namespace GlLib.Common.Items
{
    public class Item
    {
        public int id;

        public ItemRarity rarity;

        public string sprite;
        public ItemType type;
        public string unlocalizedName;

        public Item(string _name, ItemRarity _rarity, ItemType _type = ItemType.Varia)
        {
            rarity = _rarity;
            unlocalizedName = _name;
            type = _type;
        }

        public virtual string GetName(ItemStack _itemStack)
        {
            return unlocalizedName;
        }

        public virtual string GetTextureName(ItemStack _itemStack)
        {
            return "items/"+unlocalizedName + ".png";
        }

        public virtual ISprite GetItemSprite(ItemStack _itemStack)
        {
            return new PictureSprite(GetTextureName(_itemStack));
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
            return unlocalizedName;
        }
    }
}
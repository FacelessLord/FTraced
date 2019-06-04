using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;

namespace GlLib.Common.Items
{
    public class Item
    {
        public int id;

        public ItemRarity rarity;

        public string sprite;
        public ItemType type;
        public string unlocalizedName;

        public Texture texture = Textures.monochromatic;

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

        public Item SetItemTexture(Texture _texture)
        {
            texture = _texture;
            return this;
        }

        public virtual Sprite GetItemSprite(ItemStack _itemStack)
        {
            return new PictureSprite(texture);
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
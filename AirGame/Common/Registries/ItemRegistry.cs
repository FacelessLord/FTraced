using GlLib.Client.Graphic;
using GlLib.Common.Items;

namespace GlLib.Common.Registries
{
    public class ItemRegistry
    {
        public Item apple =
            new Item("item.apple", ItemRarity.Legendary, ItemType.Food).SetItemTexture(Textures.goldApple);


        public GameRegistry registry;

        public Item ring =
            new Item("item.ring", ItemRarity.Legendary, ItemType.Ring).SetItemTexture(Textures.goldRubyRing);

        public Item sword =
            new Item("item.sword", ItemRarity.Legendary, ItemType.Weapon).SetItemTexture(Textures.commonSword);

        public Item varia =
            new Item("item.varia", ItemRarity.Legendary).SetItemTexture(Textures.varia);

        public Item dawnArmor =
            new Item("item.dawn_chestplate", ItemRarity.Legendary, ItemType.Armor).SetItemTexture(
                Textures.dawnChestplate);

        public Item dawnBoots =
            new Item("item.dawn_boots", ItemRarity.Legendary, ItemType.Boots).SetItemTexture(Textures.dawnBoots);

        public Item dawnShield =
            new Item("item.dawn_shield", ItemRarity.Legendary, ItemType.Shield).SetItemTexture(Textures.dawnShield);

        public Item dawnBlade =
            new Item("item.dawn_blade", ItemRarity.Legendary, ItemType.Weapon).SetItemTexture(Textures.dawnBlade);

        public ItemRegistry(GameRegistry _registry)
        {
            registry = _registry;
        }

        public void Register()
        {
            registry.RegisterItem(sword);
            registry.RegisterItem(apple);
            registry.RegisterItem(ring);
            registry.RegisterItem(varia);

            registry.RegisterItem(dawnArmor);
            registry.RegisterItem(dawnBoots);
            registry.RegisterItem(dawnShield);
            registry.RegisterItem(dawnBlade);
        }
    }
}
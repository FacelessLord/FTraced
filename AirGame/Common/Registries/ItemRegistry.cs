using GlLib.Client.Graphic;
using GlLib.Common.Items;

namespace GlLib.Common.Registries
{
    public class ItemRegistry
    {
        public Item apple =
            new Item("item.apple", ItemRarity.Legendary, ItemType.Food).SetItemTexture(Textures.goldApple);

        public Item dawnArmor =
            new Item("item.dawn_chestplate", ItemRarity.Legendary, ItemType.Armor).SetItemTexture(
                Textures.dawnChestplate);

        public Item dawnBlade =
            new ItemTool("item.dawn_blade", ItemRarity.Legendary, new ToolMaterial(23, 0, 5, 20)).SetItemTexture(
                Textures.dawnBlade);

        public Item dawnBoots =
            new Item("item.dawn_boots", ItemRarity.Legendary, ItemType.Boots).SetItemTexture(Textures.dawnBoots);

        public Item dawnShield =
            new Item("item.dawn_shield", ItemRarity.Legendary, ItemType.Shield).SetItemTexture(Textures.dawnShield);


        public GameRegistry registry;

        public Item ring =
            new Item("item.ring", ItemRarity.Legendary, ItemType.Ring).SetItemTexture(Textures.goldRubyRing);

        public Item varia =
            new Item("item.varia", ItemRarity.Legendary).SetItemTexture(Textures.varia);

        public ItemRegistry(GameRegistry _registry)
        {
            registry = _registry;
        }

        public void Register()
        {
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
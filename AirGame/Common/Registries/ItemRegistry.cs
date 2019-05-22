using GlLib.Common.Items;

namespace GlLib.Common.Registries
{
    public class ItemRegistry
    {
        public Item apple =
            new Item("item.apple", ItemRarity.Legendary, ItemType.Food);

        public Item armor =
            new Item("item.armor", ItemRarity.Legendary, ItemType.Armor);

        public GameRegistry registry;

        public Item ring =
            new Item("item.ring", ItemRarity.Legendary, ItemType.Ring);

        public Item sword =
            new Item("item.sword", ItemRarity.Legendary, ItemType.Weapon);

        public Item varia =
            new Item("item.varia", ItemRarity.Legendary);
        
        public Item dawnArmor = new Item("item.dawn_chestplate", ItemRarity.Legendary, ItemType.Armor);
        public Item dawnBoots = new Item("item.dawn_boots", ItemRarity.Legendary, ItemType.Boots);
        public Item dawnShield = new Item("item.dawn_shield", ItemRarity.Legendary, ItemType.Shield);
        public Item dawnBlade = new Item("item.dawn_blade", ItemRarity.Legendary, ItemType.Weapon);

        public ItemRegistry(GameRegistry _registry)
        {
            registry = _registry;
        }

        public void Register()
        {
            registry.RegisterItem(sword);
            registry.RegisterItem(apple);
            registry.RegisterItem(ring);
            registry.RegisterItem(armor);
            registry.RegisterItem(varia);
            
            registry.RegisterItem(dawnArmor);
            registry.RegisterItem(dawnBoots);
            registry.RegisterItem(dawnShield);
            registry.RegisterItem(dawnBlade);
        }
    }
}
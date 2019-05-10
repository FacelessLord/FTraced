using GlLib.Common.Items;

namespace GlLib.Common.Registries
{
    public class ItemRegistry
    {
        public GameRegistry registry;

        public Item sword = 
            new Item("item.sword", ItemRarity.Legendary, ItemType.Weapon);
        public Item apple = 
            new Item("item.apple", ItemRarity.Legendary, ItemType.Food);
        public Item ring = 
            new Item("item.ring", ItemRarity.Legendary, ItemType.Ring);
        public Item armor = 
            new Item("item.armor", ItemRarity.Legendary, ItemType.Armor);
        public Item varia = 
            new Item("item.varia", ItemRarity.Legendary);

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
        }
    }
}
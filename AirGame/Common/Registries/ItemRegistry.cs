using GlLib.Common.Items;

namespace GlLib.Common.Registries
{
    public class ItemRegistry
    {
        public GameRegistry registry;

        public Item test = 
            new Item("item.test", "one_and_a_half_sword.png", ItemRarity.Legendary, ItemType.Weapon);

        public ItemRegistry(GameRegistry _registry)
        {
            registry = _registry;
        }

        public void Register()
        {
            registry.RegisterItem(test);
        }
    }
}
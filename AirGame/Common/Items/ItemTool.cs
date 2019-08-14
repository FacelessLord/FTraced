using System.Collections.Generic;

namespace GlLib.Common.Items
{
    public class ItemTool : Item
    {
        public ToolMaterial material;
        public ItemTool(string _name, ItemRarity _rarity, ToolMaterial _material) : base(_name, _rarity)
        {
            material = _material;
        }
        
        public override void AddInformation(ItemStack _itemStack, List<string> _tooltip)
        {
            _tooltip.Add("");
            _tooltip.Add("+"+material.meleeAttack + " Melee damage");
            _tooltip.Add("+"+material.rangedAttack + " Ranged damage");
            _tooltip.Add("+"+material.speed);
            _tooltip.Add("+"+material.enchantability);
        }
    }
}
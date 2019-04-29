using GlLib.Common.Items;

namespace GlLib.Client.API.Inventory
{
    public class PlayerInventory : InventoryList
    {
        public override int GetMaxSize()
        {
            return 2;
        }
        
        public override string GetInventoryName()
        {
            return "inventory.player";
        }
    }
}
using GlLib.Client.API.Inventory;

namespace GlLib.Common.Api.Inventory
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
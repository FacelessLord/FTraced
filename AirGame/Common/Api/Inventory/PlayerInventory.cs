namespace GlLib.Common.Api.Inventory
{
    public class PlayerInventory : InventoryList
    {
        public override int GetMaxSize()
        {
            return 10;
        }

        public override string GetInventoryName()
        {
            return "inventory.player";
        }
    }
}
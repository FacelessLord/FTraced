using GlLib.Client.API.Inventory;

namespace GlLib.Client.API.Gui
{
    public class GuiInventory : Gui
    {
        public IInventory inventory;

        public GuiInventory(IInventory _inventory)
        {
            inventory = _inventory;
        }
    }
}
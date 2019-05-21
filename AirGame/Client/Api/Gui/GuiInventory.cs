using GlLib.Common.Api.Inventory;

namespace GlLib.Client.API.Gui
{
    public class GuiInventory : GuiFrame
    {
        public IInventory inventory;

        public GuiInventory(IInventory _inventory)
        {
            inventory = _inventory;
        }
    }
}
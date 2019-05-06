using GlLib.Common.Api.Inventory;

namespace GlLib.Client.API.Gui
{
    public class GuiFrameInventory : GuiFrame
    {
        public IInventory inventory;

        public GuiFrameInventory(IInventory _inventory)
        {
            inventory = _inventory;
        }
    }
}
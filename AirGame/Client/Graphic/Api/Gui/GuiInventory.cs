using GlLib.Common.Api.Inventory;
using GlLib.Common.Items;

namespace GlLib.Client.Api.Gui
{
    public class GuiInventory : GuiFrame
    {
        public IInventory inventory;

        public GuiInventory(IInventory _inventory)
        {
            inventory = _inventory;
        }
        
        public static void AddSlotType(IInventory _inv, int _slotId, int _x, int _y, int _slotSize, GuiPanel _panel)
        {
            var slotRect = new GuiRectangle(_x, _y, _slotSize, _slotSize);
            _panel.Add(slotRect);
            var slot = new GuiSlotTypeRenderer(_inv, _slotId, _x, _y);
            _panel.Add(slot);
        }
        
        public static void AddSlotWithEquipmentType(IInventory _inv, int _slotId, ItemType _type, int _x, int _y, int _slotSize, GuiPanel _panel)
        {
            var slotRect = new GuiRectangle(_x, _y, _slotSize, _slotSize);
            _panel.Add(slotRect);
            var type = new GuiEquipmentTypeRenderer(_type, _x, _y);
            _panel.Add(type);
            var slot = new GuiSlotTypeRenderer(_inv, _slotId, _x, _y);
            _panel.Add(slot);
        }
    }
}
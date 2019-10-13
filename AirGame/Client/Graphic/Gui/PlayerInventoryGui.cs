using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Api.Gui;
using GlLib.Common;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class PlayerInventoryGui : GuiInventory
    {
        public Player player;
        public List<GuiSlotSign> signs = new List<GuiSlotSign>();
        private GuiSign tooltip;

        public PlayerInventoryGui(Player _p) : base(_p.inventory)
        {
//            AddRectangle(100, 16, 4 * w / 9, 2 * h / 5);
            player = _p;
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            AddInventory(_p.inventory, 100, 16);
            AddEquipment(_p.equip, 100 + 4 * w / 9, 16);
        }

        private int AddInventory(IInventory _inv, int _x, int _y)
        {
            var slotSize = GuiSlot.SlotSize;
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = 4;
            var ph = 6 * d + slotSize * 4;
            var panel = new GuiPanel(_x, _y, 50 + 3 * w / 9, ph);
            Add(panel);
            panel.bar = new GuiScrollBar(panel.width - 50, 0, 50, panel.height);
            var dy = slotSize / 2;
            for (var i = 0; i < _inv.GetMaxSize(); i++)
            {
                var rect = new GuiRectangle(slotSize / 2, dy * i,
                    panel.width - slotSize / 2 - panel.bar.width - d, slotSize / 2);
                panel.Add(rect);
                var slotRect = new GuiRectangle(0, dy * i,
                    slotSize / 2, slotSize / 2);
                panel.Add(slotRect);
                var slot = new GuiSlotTypeRenderer(_inv, i, 0, dy * i, slotSize / 2);
                panel.Add(slot);
                var text = new GuiSlotSign(_inv, 12, i, slotSize / 2, dy * i,
                    4 * w / 9 - d - slotSize * 5 / 4, slotSize / 2);
                panel.Add(text);
                signs.Add(text);
            }

            panel.bar.maxValue = (int) (panel.GetPanelBox().Height - panel.GetViewbox().Height);

            var itemPanel = new GuiPanel(_x, _y + 16 + 2 * h / 5, 50 + 3 * w / 9, h / 5);
            Add(itemPanel);

            var dh = itemPanel.height / 2 - GuiSlot.SlotSize;
            var itemSlot = new GuiSlot(_inv, 5, dh);
            itemPanel.Add(itemSlot);
            tooltip = new GuiSign("", 16, GuiSlot.SlotSize * 2 + 5, dh, 0, 0);
            itemPanel.Add(tooltip);
            return dh;
        }

        private int AddEquipment(IInventory _inv, int _x, int _y)
        {
            var slotSize = GuiSlot.SlotSize;
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var d = 4;
            var pw = 5 * d + slotSize * 3;
            var ph = 6 * d + slotSize * 4;
            var panel = new GuiPanel(_x, _y, pw, ph);
            Add(panel);
            AddSlotWithEquipmentType(_inv, 0, ItemType.Weapon, d, d, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 1, ItemType.Shield, 3 * d + slotSize * 2, d, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 2, ItemType.Helmet, 2 * d + slotSize, d, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 3, ItemType.Armor, 2 * d + slotSize, 2 * d + slotSize, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 4, ItemType.Belt, 2 * d + slotSize,
                3 * d + slotSize * 2, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 5, ItemType.Boots, 2 * d + slotSize,
                4 * d + slotSize * 3, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 6, ItemType.Ring, 2 * d + slotSize,
                3 * d + slotSize * 2, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 7, ItemType.Ring, 3 * d + 2 * slotSize,
                3 * d + slotSize * 2, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 8, ItemType.Varia, 3 * d + 2 * slotSize,
                2 * d + slotSize, slotSize, panel);

            var itemPanel = new GuiPanel(_x, _y + 16 + 2 * h / 5, 50 + 3 * w / 9, h / 5);
            Add(itemPanel);

            var dh = itemPanel.height / 2 - GuiSlot.SlotSize;
            var itemSlot = new GuiSlot(_inv, 5, dh);
            itemPanel.Add(itemSlot);
            return dh;
        }

        public override void Update(GameWindow _window)
        {
            base.Update(_window);

            var stack = inventory.GetStackInSlot(inventory.GetSelectedSlot());
            if (stack != null) tooltip.text = stack.GetTooltip().Aggregate((_a, _b) => _a + "\n" + _b);
        }
    }
}
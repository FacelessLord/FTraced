using System.Collections.Generic;
using GlLib.Client.Api.Gui;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Common;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using OpenTK;

namespace GlLib.Client.Graphic.Gui
{
    public class PlayerInventoryGui : GuiInventory
    {
        public List<GuiSlotSign> signs = new List<GuiSlotSign>();
        public Player player;

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
            var panel = new GuiPanel(_x, _y, 50 + 3 * w / 9, 2 * h / 5);
            Add(panel);
            panel.bar = new GuiScrollBar(panel.width - 50, 0, 50, panel.height);
            for (var i = 0; i < _inv.GetMaxSize(); i++)
            {
                var dy = slotSize + 2;
                var rect = new GuiRectangle(slotSize, dy * i,
                    panel.width - slotSize - panel.bar.width - d, slotSize);
                panel.Add(rect);
                var slotRect = new GuiRectangle(0, dy * i,
                    slotSize, slotSize);
                panel.Add(slotRect);
                var slot = new GuiSlotTypeRenderer(_inv, i, 0, dy * i);
                panel.Add(slot);
                var text = new GuiSlotSign(_inv, i, slotSize, dy * i,
                    4 * w / 9 - d - slotSize * 5 / 2, slotSize);
                panel.Add(text);
                signs.Add(text);
            }

            panel.bar.maxValue = (int) (panel.GetPanelBox().Height - panel.GetViewbox().Height);

            var itemPanel = new GuiPanel(_x, _y + 16 + 2 * h / 5, 50 + 3 * w / 9, h / 5);
            Add(itemPanel);

            var dh = itemPanel.height / 2 - GuiSlot.SlotSize;
            var itemSlot = new GuiSlot(_inv, 5, dh);
            itemPanel.Add(itemSlot);
            return dh;
        }

        private int AddEquipment(IInventory _inv, int _x, int _y)
        {
            var slotSize = GuiSlot.SlotSize;
            var w = Proxy.GetWindow().Width;
            var h = Proxy.GetWindow().Height;
            var pw = 50 + 3 * w / 9;
            var panel = new GuiPanel(_x, _y, pw, 2 * h / 5);
            Add(panel);

            AddSlotWithEquipmentType(_inv, 0, ItemType.Weapon, pw / 2 - slotSize * 2, 50, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 1, ItemType.Shield, pw / 2 + slotSize, 50, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 2, ItemType.Helmet, pw / 2 - slotSize / 2, 50, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 3, ItemType.Armor, pw / 2 - slotSize / 2, 50 + slotSize, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 4, ItemType.Belt, pw / 2 - slotSize / 2,
                50 + slotSize * 2, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 5, ItemType.Boots, pw / 2 - slotSize / 2,
                50 + slotSize * 3, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 6, ItemType.Ring, pw / 2 - slotSize * 2,
                50 + slotSize * 2, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 7, ItemType.Ring, pw / 2 + slotSize,
                50 + slotSize * 2, slotSize, panel);
            AddSlotWithEquipmentType(_inv, 8, ItemType.Varia, pw / 2 + slotSize * 2,
                50 + slotSize * 2, slotSize, panel);

            var itemPanel = new GuiPanel(_x, _y + 16 + 2 * h / 5, 50 + 3 * w / 9, h / 5);
            Add(itemPanel);

            var dh = itemPanel.height / 2 - GuiSlot.SlotSize;
            var itemSlot = new GuiSlot(_inv, 5, dh);
            itemPanel.Add(itemSlot);
            return dh;
        }
    }
}
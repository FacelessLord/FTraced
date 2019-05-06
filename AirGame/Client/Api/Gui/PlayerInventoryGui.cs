using System.Collections.Generic;
using System.Net.Sockets;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using GlLib.Utils;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class PlayerFrameInventoryGuiFrame : GuiFrameInventory
    {
        public List<GuiSign> signs = new List<GuiSign>();

        public PlayerFrameInventoryGuiFrame(Player _p) : base(_p.inventory)
        {
            var w = GraphicWindow.instance.Width;
            var h = GraphicWindow.instance.Height;
//            AddRectangle(100, 16, 4 * w / 9, 2 * h / 5);

            var d = 4;
            var slotSize = GuiSlotTypeRenderer.SlotSize;
            var texture = Vertexer.LoadTexture("gui/window_back.png");
            var background =
                new TextureLayout(texture, 0, 0, 96, 96, 3, 3);
            var panel = new GuiPanel(100, 16, 50 + 4 * w / 9, 2 * h / 5);
            Add(panel);
            panel.bar = new GuiScrollBar(panel.height, panel.width - 50, 0, 50, panel.height);
            for (int i = 0; i < _p.inventory.GetMaxSize(); i++)
            {
                int dy = slotSize + 2;
                var rect = new GuiRectangle(background, slotSize, dy * i,
                    panel.width - slotSize - panel.bar.width - d, slotSize);
                panel.Add(rect);
                var slotRect = new GuiRectangle(background, 0, dy * i,
                    slotSize, slotSize);
                panel.Add(slotRect);
                var slot = new GuiSlotTypeRenderer(inventory, i, 0, dy * i);
                panel.Add(slot);
                var text = new GuiSign("", slotSize, dy * i,
                    4 * w / 9 - d - slotSize * 5 / 2, slotSize);
                panel.Add(text);
                signs.Add(text);
            }

            panel.bar.maxValue = (int) (panel.GetPanelBox().Height - panel.GetViewbox().Height);

            var itemPanel = new GuiPanel(100, 32 + 2 * h / 5, 50 + 4 * w / 9, h / 5);
            Add(itemPanel);

            var dh = (itemPanel.height - GuiSlot.SlotSize) / 2;
            var itemSlot = new GuiPlayerSlot(_p.inventory, 5, dh);
            itemPanel.Add(itemSlot);
        }


        public override void Update(GameWindow _window)
        {
            base.Update(_window);
            for (int i = 0; i < signs.Count; i++)
            {
                if (inventory.GetStackInSlot(i) is ItemStack stack)
                    signs[i].text = stack.item.GetName(stack);
                else
                    signs[i].text = "";
            }
        }
    }
}
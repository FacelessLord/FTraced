using System.Collections.Generic;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.API.Inventory;
using GlLib.Client.Graphic;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Items;
using GlLib.Utils;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class PlayerInventoryGui : GuiInventory
    {
        public List<GuiSign> signs = new List<GuiSign>();

        public PlayerInventoryGui(Player _p) : base(_p.inventory)
        {
            var w = GraphicWindow.instance.Width;
            var h = GraphicWindow.instance.Height;
//            AddRectangle(100, 16, 4 * w / 9, 2 * h / 5);

            var d = 4;
            var slotSize = GuiSlot.SlotSize;
            var texture = Vertexer.LoadTexture("gui/window_back.png");
            var background =
                new TextureLayout(texture, 0, 0, 96, 96, 3, 3);
            var panel = new GuiPanel(100, 16, 50 + 4 * w / 9, 2 * h / 5);
            Add(panel);
            panel.bar = new GuiScrollBar(panel.height, panel.width - 50, 0, 50, panel.height);

            for (int i = 0; i < _p.inventory.GetMaxSize(); i++)
            {
                var itemPanel = new GuiPanel(d, d+slotSize * i, panel.width - d*2 - panel.bar.width, slotSize);
//                itemPanel.enableBackground = false;
                panel.Add(itemPanel);
                var rect = new GuiRectangle(background, slotSize, slotSize / 4,
                    4 * w / 9 - d - slotSize * 5 / 4, slotSize / 2);
                itemPanel.Add(rect);
//                var slot = new GuiSlot(_p.inventory, i, 0, 0);
//                panel.Add(slot);
//                var text = new GuiSign("", slotSize, slotSize / 4,
//                    4 * w / 9 - d - slotSize * 5 / 4, slotSize / 2);
//                panel.Add(text);
//                signs.Add(text);
            }
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
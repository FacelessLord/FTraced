using System.Collections.Generic;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.API.Inventory;
using GlLib.Client.Graphic;
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
            AddRectangle(100, 16, 4 * w / 9, 2 * h / 5);

            var d = 4;
            var slotSize = GuiSlot.SlotSize;
            var texture = Vertexer.LoadTexture("gui/text_back.png");
            var background =
                new TextureLayout(texture, 0, 0, 96, 96, 3, 3);

            for (int i = 0; i < _p.inventory.GetMaxSize(); i++)
            {
                var rect = new GuiRectangle(background, 100 + slotSize, 16 + d + (slotSize + d) * i + slotSize / 4,
                    4 * w / 9 - d - slotSize * 5 / 4, slotSize / 2);
                Add(rect);
                var slot = new GuiSlot(_p.inventory, i, 100 + d, 16 + d + (slotSize + d) * i);
                Add(slot);
                var text = AddText("", 100 + slotSize, 16 + d + (slotSize + d) * i + slotSize / 4,
                    4 * w / 9 - d - slotSize * 5 / 4, slotSize / 2);
                signs.Add(text);
            }

            var button = new GuiButton(200, 100, 200, 50);
            Add(button);
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
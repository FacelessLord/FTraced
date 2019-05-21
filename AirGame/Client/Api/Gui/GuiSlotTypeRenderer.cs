using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiSlotTypeRenderer : GuiObject
    {
        public const int SlotSize = 32;
        public IInventory inventory;
        public int slot;

        public TextureLayout slotTexture;

        public GuiSlotTypeRenderer(IInventory _inventory, int _slot, int _x, int _y) : base(_x, _y, SlotSize,
            SlotSize)
        {
            slotTexture = new TextureLayout("gui/item_classes.png", 4, 4);
            inventory = _inventory;
            slot = _slot;
        }

        public GuiSlotTypeRenderer(IInventory _inventory, int _slot, int _x, int _y, Color _color) : base(
            _x, _y, SlotSize, SlotSize, _color)
        {
            slotTexture = new TextureLayout("gui/item_classes.png", 4, 4);
            inventory = _inventory;
            slot = _slot;
        }

        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            if (inventory is PlayerInventory pi && _button == MouseButton.Right)
            {
                pi.currentSlot = slot;
            }
            else
            {
                if (_gui.SelectedSlot != null)
                {
                    var slotStack = inventory.GetStackInSlot(slot);
                    var selectedStack = _gui.SelectedSlot.GetStack();
                    _gui.SelectedSlot.SetStack(slotStack);
                    inventory.SetItemStack(selectedStack, slot);
                    _gui.SelectedSlot = null;
                }
                else
                {
                    _gui.SelectedSlot = new Slot(inventory, slot);
                }
            }

            return this;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            slotTexture.texture.Bind();
            if (inventory.GetStackInSlot(slot) != null)
                Vertexer.DrawLayoutPart(slotTexture, x + 1, y + 1, (int) inventory.GetStackInSlot(slot).item.type,
                    width, height);
        }
    }
}
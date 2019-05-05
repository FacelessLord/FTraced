using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.API.Inventory;
using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Items;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiSlotTypeRenderer : GuiObject
    {
        public IInventory inventory;
        public int slot;
        public const int SlotSize = 32;

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

        public override GuiObject OnMouseClick(GameWindow _window, MouseButton _button, int _mouseX, int _mouseY)
        {
            if (inventory is PlayerInventory pi)
            {
                pi.currentSlot = slot;
            }
            return this;
        }
        
        public TextureLayout slotTexture;

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            if(inventory.GetStackInSlot(slot) != null)
            {
                GL.PushMatrix();
                GL.Translate(x + 1, y + 1, 0);

                GuiUtils.DrawLayoutPart(slotTexture, (int) inventory.GetStackInSlot(slot).item.type, width, height);

                GL.PopMatrix();
            }
        }
    }
}
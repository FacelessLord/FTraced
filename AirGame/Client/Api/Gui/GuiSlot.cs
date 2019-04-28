using GlLib.Client.API.Inventory;
using GlLib.Client.Graphic;
using GlLib.Common.Items;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiSlot : GuiObject
    {
        public IInventory inventory;
        public int slot;
        public const int SlotStandartSize = 16;

        public GuiSlot(IInventory _inventory, int _slot, int _x, int _y) : base(_x, _y, SlotStandartSize,
            SlotStandartSize)
        {
            slotTexture = Vertexer.LoadTexture("gui/slot.png");
            inventory = _inventory;
            slot = _slot;
        }

        public GuiSlot(IInventory _inventory, int _slot, int _x, int _y, Color _color) : base(
            _x, _y, SlotStandartSize, SlotStandartSize, _color)
        {
            slotTexture = Vertexer.LoadTexture("gui/slot.png");
            inventory = _inventory;
            slot = _slot;
        }

        public Texture slotTexture;

        public override void Render(GameWindow _window, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Translate(x, y, 0);

            Vertexer.BindTexture(slotTexture);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(x, y, 0, 0);
            Vertexer.VertexWithUvAt(x + width, y, 1, 0);
            Vertexer.VertexWithUvAt(x + width, y + height, 1, 1);
            Vertexer.VertexWithUvAt(x, y + height, 0, 1);

            Vertexer.Draw();

            ItemStack stack = inventory.GetStackInSlot(slot);
            stack?.item.GetItemSprite(stack).Render();

            GL.PopMatrix();
        }
    }
}
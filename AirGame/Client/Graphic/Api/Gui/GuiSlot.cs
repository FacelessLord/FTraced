using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Gui
{
    public class GuiSlot : GuiObject
    {
        public const int SlotSize = 48;
        public IInventory inventory;

        public Texture slotTexture;

        public GuiSlot(IInventory _inventory, int _x, int _y) : base(_x, _y, SlotSize,
            SlotSize)
        {
            slotTexture = Textures.slot;
            inventory = _inventory;
        }

        public GuiSlot(PlayerInventory _inventory, int _x, int _y, Color _color) : base(
            _x, _y, SlotSize, SlotSize, _color)
        {
            slotTexture = Textures.slot;
            inventory = _inventory;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Translate(x, y, 0);
            GL.Scale(2, 2, 1);

            Vertexer.BindTexture(slotTexture);

            Vertexer.DrawSquare(0, 0, width, height);
            GL.Translate(width / 2d, height / 2d, 0);
            var stack = inventory.GetStackInSlot(inventory.GetSelectedSlot());
            stack?.item.GetItemSprite(stack).Render();
            GL.PopMatrix();
        }
    }
}
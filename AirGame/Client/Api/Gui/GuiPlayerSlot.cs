using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.API.Gui
{
    public class GuiPlayerSlot : GuiObject
    {
        public const int SlotSize = 96;
        public PlayerInventory inventory;

        public Texture slotTexture;

        public GuiPlayerSlot(PlayerInventory _inventory, int _x, int _y) : base(_x, _y, SlotSize,
            SlotSize)
        {
            slotTexture = Vertexer.LoadTexture("gui/slot.png");
            inventory = _inventory;
        }

        public GuiPlayerSlot(PlayerInventory _inventory, int _x, int _y, Color _color) : base(
            _x, _y, SlotSize, SlotSize, _color)
        {
            slotTexture = Vertexer.LoadTexture("gui/slot.png");
            inventory = _inventory;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            GL.PushMatrix();
            GL.Translate(x, y, 0);

            Vertexer.BindTexture(slotTexture);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(0, 0, 0, 0);
            Vertexer.VertexWithUvAt(width, 0, 1, 0);
            Vertexer.VertexWithUvAt(width, height, 1, 1);
            Vertexer.VertexWithUvAt(0, height, 0, 1);

            Vertexer.Draw();

            var stack = inventory.GetStackInSlot(inventory.currentSlot);
            stack?.item.GetItemSprite(stack).Render();

            GL.PopMatrix();
        }
    }
}
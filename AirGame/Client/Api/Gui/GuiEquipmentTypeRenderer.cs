using GlLib.Client.Api.Sprites;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Items;
using GlLib.Utils;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiEquipmentTypeRenderer : GuiObject
    {
        public ItemType type;

        public TextureLayout slotTexture;

        public GuiEquipmentTypeRenderer(ItemType _type, int _x, int _y) : base(_x, _y, GuiSlot.SlotSize,
            GuiSlot.SlotSize)
        {
            slotTexture = new TextureLayout("gui/equipment_sub.png", 4, 4);
            type = _type;
        }

        public GuiEquipmentTypeRenderer(ItemType _type, int _x, int _y, Color _color) : base(
            _x, _y, GuiSlot.SlotSize, GuiSlot.SlotSize, _color)
        {
            slotTexture = new TextureLayout("gui/equipment_sub.png", 4, 4);
            type = _type;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            slotTexture.texture.Bind();
            Vertexer.DrawLayoutPart(slotTexture, x, y, (int) type, width, height);
        }
    }
}
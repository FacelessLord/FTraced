using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Items;
using OpenTK;

namespace GlLib.Client.Api.Gui
{
    public class GuiEquipmentTypeRenderer : GuiObject
    {
        public TextureLayout slotTexture;
        public ItemType type;

        public GuiEquipmentTypeRenderer(ItemType _type, int _x, int _y) : base(_x, _y, GuiSlot.SlotSize,
            GuiSlot.SlotSize)
        {
            slotTexture = new TextureLayout(Textures.equipBackground, 4, 4);
            type = _type;
        }

        public GuiEquipmentTypeRenderer(ItemType _type, int _x, int _y, Color _color) : base(
            _x, _y, GuiSlot.SlotSize, GuiSlot.SlotSize, _color)
        {
            slotTexture = new TextureLayout(Textures.equipBackground, 4, 4);
            type = _type;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            slotTexture.texture.Bind();
            Vertexer.DrawLayoutPart(slotTexture, x, y, (int) type, width, height);
        }
    }
}
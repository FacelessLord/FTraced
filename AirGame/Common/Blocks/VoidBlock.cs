using GlLib.Client.API;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Blocks
{
    public class VoidBlock : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.void";
        public override string TextureName { get; internal set; } = "null.png";

        public override bool RequiresSpecialRenderer(World _world, int _x, int _y)
        {
            return true;
        }

        public override IBlockRenderer GetSpecialRenderer(World _world, int _x, int _y)
        {
            return new VoidRenderer();
        }

        public override AxisAlignedBb GetCollisionBox()
        {
//            return base.GetCollisionBox();
            return new AxisAlignedBb(0, 0, 1, 1);
        }
    }
}
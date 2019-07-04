using GlLib.Client.Api.Renderers;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;
using GlLib.Utils.Math;

namespace GlLib.Common.Blocks
{
    public class CastleBlock : TerrainBlock
    {
        public override string Name { get; protected set; } = "block.castle";
        public override string TextureName { get; internal set; } = ".";
        
        public override bool RequiresSpecialRenderer(World _world, int _x, int _y)
        {
            return true;
        }

        public override IBlockRenderer GetSpecialRenderer(World _world, int _x, int _y)
        {
            return new CastleRenderer();
        }

        public override AxisAlignedBb GetCollisionBox()
        {
            return new AxisAlignedBb(0, 0, 1, 1);
        }
    }
}
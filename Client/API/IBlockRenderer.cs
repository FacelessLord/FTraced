using GlLib.Common.Map;

namespace GlLib.Client.API
{
    public interface IBlockRenderer
    {
        void Render(World world, int x, int y);
    }
}
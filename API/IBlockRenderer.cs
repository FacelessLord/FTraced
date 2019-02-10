using GlLib.Map;

namespace GlLib.API
{
    public interface IBlockRenderer
    {
        void Render(World world, int x, int y);
    }
}
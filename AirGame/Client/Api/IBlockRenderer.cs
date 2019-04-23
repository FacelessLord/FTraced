using GlLib.Common.Map;

namespace GlLib.Client.API
{
    public interface IBlockRenderer
    {
        void Render(World _world, int _x, int _y);
    }
}
using GlLib.Common.Map;

namespace GlLib.Client.API
{
    public interface IBlockRenderer
    {
        void Render(ClientWorld world, int x, int y);
    }
}
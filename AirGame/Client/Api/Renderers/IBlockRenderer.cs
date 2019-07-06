using GlLib.Common.Map;

namespace GlLib.Client.Api.Renderers
{
    public interface IBlockRenderer
    {
        void Render(World _world, int _x, int _y);
    }
}
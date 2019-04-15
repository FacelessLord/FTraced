using GlLib.Client.API;

namespace GlLib.Common.Map
{
    public abstract class TerrainBlock
    {
        public int id = -1;

        /// <summary>
        ///     name in format : [mod:]block.[blockset.]blockname[.subtype]
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        public abstract string GetTextureName(World _world, int _x, int _y);

        public virtual bool RequiresSpecialRenderer(World _world, int _x, int _y)
        {
            return false;
        }

        public virtual IBlockRenderer GetSpecialRenderer(World _world, int _x, int _y)
        {
            return null;
        }
    }
}
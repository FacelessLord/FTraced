using GlLib.Client.API;
using GlLib.Client.Graphic;

namespace GlLib.Common.Map
{
    public abstract class TerrainBlock
    {
        /// <summary>
        /// name in format : [mod:]block.[blockset.]blockname[.subtype]
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        public int _id = -1;
        public abstract string GetTextureName(World world,int x, int y);

        public virtual bool RequiresSpecialRenderer(World world,int x,int y)
        {
            return false;
        }

        public virtual IBlockRenderer GetSpecialRenderer(World world,int x,int y)
        {
            return null;
        }
    }
}
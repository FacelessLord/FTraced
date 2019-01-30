using GlLib.Graphic;

namespace GlLib.Map
{
    public abstract class TerrainBlock
    {
        /// <summary>
        /// name in format : [mod:]block.[blockset.]blockname or 
        /// </summary>
        /// <returns></returns>
        public abstract string GetName(); 
        public abstract Texture GetTexture(int x, int y);
    }
}
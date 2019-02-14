using GlLib.Common.Entities;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Registries
{
    public class Entities
    {
        public static void Register()
        {
            GameRegistry.RegisterEntity("entity.player", typeof(Player));
        }
    }
}
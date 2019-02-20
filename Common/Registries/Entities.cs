using GlLib.Common.Entities;
using GlLib.Common.Map;

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
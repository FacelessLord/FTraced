using GlLib.Entities;
using GlLib.Map;
using GlLib.Utils;

namespace GlLib.Registries
{
    public class Entities
    {
        public static void Register()
        {
            GameRegistry.RegisterEntity(new Entity(Core.Core.World,new RestrictedVector3D()));
        }
    }
}
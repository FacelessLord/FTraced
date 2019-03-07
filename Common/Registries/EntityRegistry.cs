using GlLib.Common.Entities;
using GlLib.Common.Map;

namespace GlLib.Common.Registries
{
    public class EntityRegistry
    {
        public GameRegistry registry;

        public EntityRegistry(GameRegistry registry)
        {
            this.registry = registry;
        }

        public void Register()
        {
            registry.RegisterEntity("entity.player", typeof(Player));
        }
    }
}
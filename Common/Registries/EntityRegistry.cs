using GlLib.Common.Entities;
using GlLib.Common.Map;

namespace GlLib.Common.Registries
{
    public class EntityRegistry
    {
        public EntityRegistry(GameRegistry registry)
        {
            _registry = registry;
        }
        
        public GameRegistry _registry;
        
        public void Register()
        {
            _registry.RegisterEntity("entity.player", typeof(Player));
        }
    }
}
using GlLib.Common.Entities;

namespace GlLib.Common.Registries
{
    public class EntityRegistry
    {
        public GameRegistry registry;

        public EntityRegistry(GameRegistry _registry)
        {
            registry = _registry;
        }

        public void Register()
        {
            registry.RegisterEntity("entity.null", typeof(Entity));
            registry.RegisterEntity("entity.living.player", typeof(Player));
            registry.RegisterEntity("entity.living.slime", typeof(EntitySlime));
            registry.RegisterEntity("entity.bone_pile", typeof(BonePile));
        }
    }
}
namespace GlLib.Common.Entities.Intelligence
{
    public interface
        IArtificialIntelligence //TODO maybe remove this Interface 'cause it is makes more problems than eases usage of AI
    {
        void Setup(EntityLiving _entity);
        void Update(EntityLiving _entity);
        void OnCollision(EntityLiving _entity, Entity _collider);
    }
}
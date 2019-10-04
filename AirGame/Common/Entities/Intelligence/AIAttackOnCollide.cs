using GlLib.Common.Entities.Intelligence;

namespace GlLib.Common.Entities
{
    public class AIAttackOnCollide<TTargetType> : IArtificialIntelligence where TTargetType : EntityLiving
    {
        public float Damage { get; set; }
        private readonly int UpdateFrequency = 12;

        public AIAttackOnCollide(float _damage)
        {
            Damage = _damage;
        }
        
        public void Setup(EntityLiving _entity)
        {
            
        }

        public void Update(EntityLiving _entity)
        {
            
        }

        public void OnCollision(EntityLiving _entity, Entity _collider)
        {
            if (_collider is TTargetType el
                && !el.state.Equals(EntityState.Dead)
                && !(_collider is Bat)
                && _entity.InternalTicks % UpdateFrequency == 0
                && _entity.InternalTicks > 30000000)
                el.DealDamage(Damage);
        }
    }
}
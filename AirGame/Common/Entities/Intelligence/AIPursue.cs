namespace GlLib.Common.Entities.Intelligence
{
    public class AIPursue<TTargetType> : IArtificialIntelligence where TTargetType : Entity
    {
        private readonly int UpdateFrequency = 12;
        public AISearch<TTargetType> searchAI { get; set; }
        public float Speed { get; set; } = 0.2f;

        public AIPursue(AISearch<TTargetType> _search, float _speed)
        {
            searchAI = _search;
            Speed = _speed;
        }

        public void Setup(EntityLiving _entity)
        {
        }

        public void Update(EntityLiving _entity)
        {
            var target = searchAI.Target;
            if (!(target is null))
            {
                UpdateEntityHeading(_entity, target);
                _entity.state = EntityState.DirectedAttack;
            }
        }
        
        private void UpdateEntityHeading(EntityLiving _entity, TTargetType _target)
        {
            _entity.velocity = _target.Position - _entity.Position;
            _entity.velocity.Normalize();
            _entity.velocity *= Speed;//TODO just rotation towards target or with given speed
        }

        public void OnCollision(EntityLiving _entity, Entity _collider)
        {
        }
    }
}
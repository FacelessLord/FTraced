namespace GlLib.Common.Entities.Intelligence
{
    public class AiPursue<TTargetType> : IArtificialIntelligence where TTargetType : Entity
    {
        public AiPursue(AiSearch<TTargetType> _search, float _speed = 0.2f)
        {
            SearchAi = _search;
            Speed = _speed;
        }

        public AiSearch<TTargetType> SearchAi { get; set; }
        public float Speed { get; set; }

        public void Setup(EntityLiving _entity)
        {
        }

        public void Update(EntityLiving _entity)
        {
            var target = SearchAi.Target;
            if (!(target is null))
            {
                UpdateEntityHeading(_entity, target);
                _entity.state = EntityState.DirectedAttack;
            }
        }

        public void OnCollision(EntityLiving _entity, Entity _collider)
        {
        }

        private void UpdateEntityHeading(EntityLiving _entity, TTargetType _target)
        {
            _entity.velocity = _target.Position - _entity.Position;
            _entity.velocity.Normalize();
            _entity.velocity *= Speed; //TODO just rotation towards target or with given speed
        }
    }
}
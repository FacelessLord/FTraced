namespace GlLib.Common.Entities.Intelligence
{
    public class AiJumpingPursue<TTargetType> : IArtificialIntelligence where TTargetType : Entity
    {
        private readonly int UpdateFrequency = 12;

        public AiJumpingPursue(AiSearch<TTargetType> _search)
        {
            SearchAi = _search;
        }

        public AiSearch<TTargetType> SearchAi { get; set; }

        public void Setup(EntityLiving _entity)
        {
        }

        public void Update(EntityLiving _entity)
        {
            var target = SearchAi.Target;
            if (!(target is null))
            {
                if (_entity.InternalTicks % UpdateFrequency == 0 && _entity.velocity.Length < 1)
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
            _entity.velocity /= 5; //TODO just rotation towards target or with given speed
        }
    }
}
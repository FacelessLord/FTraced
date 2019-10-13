using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Intelligence
{
    public class AiPursue<TTargetType> : IArtificialIntelligence where TTargetType : Entity
    {
        private readonly int UpdateFrequency = 4;

        public AiPursue(AiSearch<TTargetType> _search, float _speed = 0.1f)
        {
            SearchAi = _search;
            Speed = _speed;
        }

        public AiSearch<TTargetType> SearchAi { get; set; }
        public float Speed { get; set; }

        public virtual void Setup(EntityLiving _entity)
        {
        }

        public virtual void Update(EntityLiving _entity)
        {
            var target = SearchAi.Target;
            if (!(target is null))
            {
                if (_entity.InternalTicks % UpdateFrequency == 0)
                    UpdateEntityHeading(_entity, target);
                _entity.state = EntityState.DirectedAttack;
            }
        }

        public virtual void OnCollision(EntityLiving _entity, Entity _collider)
        {
        }

        protected virtual void UpdateEntityHeading(EntityLiving _entity, TTargetType _target)
        {
            PlanarVector dvelocity = _target.Position - _entity.Position;
            dvelocity.Normalize();
            dvelocity *= Speed; //TODO just rotation towards target or with given speed
            _entity.velocity += dvelocity;
        }
    }
}
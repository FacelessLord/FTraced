using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Intelligence
{
    public class AiJumpingPursue<TTargetType> : AiPursue<TTargetType> where TTargetType : Entity
    {
        private readonly int UpdateFrequency = 12;

        public AiJumpingPursue(AiSearch<TTargetType> _search, float _speed = 0.2f) : base(_search, _speed)
        {
        }

        public override void Update(EntityLiving _entity)
        {
            var target = SearchAi.Target;
            if (!(target is null))
            {
                if (_entity.InternalTicks % UpdateFrequency == 0 && _entity.velocity.Length < 0.02)
                    UpdateEntityHeading(_entity, target);
                _entity.state = EntityState.DirectedAttack;
            }
        }

        public override void OnCollision(EntityLiving _entity, Entity _collider)
        {
        }

        protected override void UpdateEntityHeading(EntityLiving _entity, TTargetType _target)
        {
            PlanarVector dvelocity = _target.Position - _entity.Position;
            dvelocity.Normalize();
            _entity.velocity += dvelocity / 5;
        }
    }
}
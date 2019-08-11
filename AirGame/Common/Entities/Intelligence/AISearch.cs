using System.Linq;

namespace GlLib.Common.Entities.Intelligence
{
    public class AISearch<TTargetType> : IArtificialIntelligence where TTargetType : Entity
    {
        public int SearchRange { get; private set; }
        public TTargetType Target { get; set; }

        public AISearch(int _range)
        {
            SearchRange = _range;
        }
        
        public void Setup(EntityLiving _entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(EntityLiving _entity)
        {
            var entities = _entity.worldObj.GetEntitiesWithinAaBb(_entity.Position.ExpandBothTo(SearchRange, SearchRange));

            //Search in radius
            Target = entities.Where(_e => _e is TTargetType).Cast<TTargetType>()
                .FirstOrDefault(_e => _e is TTargetType p && !p.state.Equals(EntityState.Dead));

            if (Target != null && (Target.Position - _entity.Position).Length > SearchRange)
            {
                _entity.state = EntityState.Idle;
                Target = null;
            }
        }

        public void OnCollision(EntityLiving _entity, Entity _collider)
        {
            
        }
    }
}
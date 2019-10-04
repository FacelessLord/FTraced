using GlLib.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Common.Api.Entity
{
    //It probably 
    internal abstract class IntelligentEntity : Entities.Entity
    {
        // TODO living action time 
        // fix some bugs
        List<Action> UpdateList;

        public void Follow(Func<Entities.Entity, bool> _target, AxisAlignedBb _range)
        {
            var target = worldObj.GetEntitiesWithinAaBb(Position.ExpandBothTo(_range.Width, _range.Height))
                .Where(_target)
                .FirstOrDefault();

            if (target is null)
                return;

            UpdateList.Add( () => moveToTarget(this, target));
        }

        public override void Update()
        {
            base.Update();

            foreach (var action in UpdateList)
            {
                action();
            }

        }

        public static Action<Entities.Entity, Entities.Entity> moveToTarget = ((_obj, _target ) =>
        {
            _obj.velocity = _target.Position - _obj.Position;
            _obj.velocity.Normalize();
            _obj.velocity /= 5;
        });

}
}

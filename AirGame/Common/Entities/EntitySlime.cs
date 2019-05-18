using System;
using System.Linq;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class EntitySlime : EntityLiving, ISmart, IAttacker
    {
        private const int UpdateFrame = 12;

        public EntitySlime()
        {
            Initialize();
        }

        public EntitySlime(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            Initialize();
        }


        private void Initialize()
        {

            SetCustomRenderer(new SlimeRenderer());
            AttackRange = 7;
            AttackValue = 5;
            AaBb = new AxisAlignedBb(-0.25, 0, 0.25, 0.5);
        }

        public override string GetName()
        {
            return "entity.living.slime";
        }

        public EntitySlime(bool _inMove, bool _inWaiting, int _attackRange, long _spawnTime)
        {
            InMove = _inMove;
            InWaiting = _inWaiting;
            Target = null;
            Initialize();
        }

        private Player Target { get; set; }
        public bool InMove { get; }
        public bool InWaiting { get; }
        public int AttackRange { get; private set; }

        public bool IsAttacking
        {
            get => !(Target is null);
        }

        public bool IsWaiting
        {
            get => (Target is null);
        }

        //public override Mov
        public override void Update()
        {
            var entities = worldObj.GetEntitiesWithinAaBb(Position.ExpandBothTo(AttackRange, AttackRange));

            if (Target is null && !(entities is null))
            {
                Target = (Player) entities
                    .FirstOrDefault(_e => _e is Player p && !p.state.Equals(EntityState.Dead));
            }

            if (InternalTicks % UpdateFrame == 0 ||
                (!(Target is null) && Target.IsDead))
            {
                Target = (Player) entities
                    .FirstOrDefault(_e => _e is Player p && !p.state.Equals(EntityState.Dead));

                if (!(Target is null) &&
                    (Target.Position - position).Length > 1)
                    MoveToTarget();
            }

            base.Update();
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (_obj is EntityLiving el
                && !(_obj is EntitySlime)
                && !el.state.Equals(EntityState.Dead)
                && InternalTicks % UpdateFrame == 0
                && InternalTicks > 30000000)
                (_obj as EntityLiving).DealDamage(AttackValue);
        }

        private void MoveToTarget()
        {
            velocity = Target.Position - position;
            velocity.Normalize();
            velocity /= 5;
        }

        public int AttackValue { get; set; }

    }
}
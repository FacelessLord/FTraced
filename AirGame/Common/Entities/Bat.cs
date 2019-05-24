using System.Linq;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    internal class Bat : EntityLiving
    {
        private const int UpdateFrame = 12;


        public Bat()
        {
            Initialize();
        }

        public Bat(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            SetCustomRenderer(new BatRenderer());
            Initialize();
        }


        private Player Target { get; set; }
        public bool InMove { get; }
        public bool InWaiting { get; }
        public int AttackRange { get; private set; }

        public bool IsAttacking => !(Target is null);

        public bool IsWaiting => Target is null;

        public int AttackValue { get; set; }


        private void Initialize()
        {
            SetCustomRenderer(new BatRenderer());
            AttackRange = 7;
            AttackValue = 5;
        }

        public override string GetName()
        {
            return "entity.living.bat";
        }

        public override void Update()
        {
            var entities = worldObj.GetEntitiesWithinAaBb(Position.ExpandBothTo(AttackRange, AttackRange));

            if (Target is null && !(entities is null))
                Target = (Player) entities
                    .FirstOrDefault(_e => _e is Player p && !p.state.Equals(EntityState.Dead));

            if (InternalTicks % UpdateFrame == 0 ||
                !(Target is null) && Target.IsDead)
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
                && !el.state.Equals(EntityState.Dead)
                && !(_obj is Bat)
                && InternalTicks % UpdateFrame == 0
                && InternalTicks > 30000000)
                (_obj as EntityLiving).DealDamage(AttackValue);
        }

        private void MoveToTarget()
        {
            velocity = (Target.Position - position).ToPlanarVector();
            velocity.Normalize();
            velocity /= 5;
        }
    }
}
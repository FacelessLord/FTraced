using System;
using System.Linq;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Api.Entity;
using GlLib.Common.Map;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK.Graphics;

namespace GlLib.Common.Entities
{
    public class EntitySlime : EntityLiving, ISmart, IAttacker
    {
        private const int UpdateFrame = 12;
        public Color4 color;

        public new EntityState state
        {
            get =>
                Target is null 
                    ? EntityState.Idle
                    : EntityState.Walk;
            private set { state = value; }
        }

        public EntitySlime()
        {
            Initialize();
        }

        public EntitySlime(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            Initialize();
        }

        public EntitySlime(bool _inMove, bool _inWaiting, int _attackRange, long _spawnTime)
        {
            InMove = _inMove;
            InWaiting = _inWaiting;
            Target = null;
            Initialize();
            state = EntityState.Idle;
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
            var r = new Random();
            color = new Color4((float) r.NextDouble(), (float) r.NextDouble(), (float) r.NextDouble(), 1);
            SetCustomRenderer(new SlimeRenderer());
            AttackRange = 2;
            AttackValue = 2;
            AaBb = new AxisAlignedBb(-0.25f, 0f, 0.25f, 0.5f);
        }

        public override string GetName()
        {
            return "entity.living.slime";
        }

        //public override Mov
        public override void Update()
        {
            //var sprite = new AlagardFontSprite();

            //sprite.DrawText("Hello, I'm Slime", 64);

            //sprite.Render((char) Proxy.GetServer().registry.entities[this]);


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
                {
                    SetState(EntityState.Walk, -1);
                    MoveToTarget();
                }

            }
            else
                SetState(EntityState.Idle, -1, true);

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
    }
}
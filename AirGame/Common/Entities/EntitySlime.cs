using System;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Api.Entity;
using GlLib.Common.Entities.Intelligence;
using GlLib.Common.Map;
using GlLib.Utils.Math;
using OpenTK.Graphics;

namespace GlLib.Common.Entities
{
    public class EntitySlime : EntityLiving, ISmart, IAttacker
    {
        public Color4 color;
        public AiAttackOnCollide<EntityPlayer> playerAttackAi;
        public AiPursue<EntityPlayer> playerPursueAi;
        public AiSearch<EntityPlayer> playerSearchAi;

        public EntitySlime()
        {
            Initialize();
        }

        public EntitySlime(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            Initialize();
        }

        public int AttackValue { get; set; } = 5;

        private void Initialize()
        {
            var r = new Random();
            color = new Color4((float) r.NextDouble(), (float) r.NextDouble(), (float) r.NextDouble(),
                (float) r.NextDouble() * 0.5f + 0.5f);
            SetCustomRenderer(new SlimeRenderer());
            AaBb = new AxisAlignedBb(-0.25f, 0, 0.25f, 0.5f);

            playerSearchAi = new AiSearch<EntityPlayer>(7);
            playerPursueAi = new AiJumpingPursue<EntityPlayer>(playerSearchAi, 0.1f);
            playerAttackAi = new AiAttackOnCollide<EntityPlayer>(AttackValue);
        }

        public override string GetName()
        {
            return "entity.living.slime";
        }

        //public override Mov
        public override void Update()
        {
            playerSearchAi.Update(this);
            playerPursueAi.Update(this);

            base.Update();
        }

        public override void OnCollideWith(Entity _obj)
        {
            playerAttackAi.OnCollision(this, _obj);
        }
    }
}
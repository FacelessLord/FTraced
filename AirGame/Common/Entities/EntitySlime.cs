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
        private const int UpdateFrequency = 12;
        public Color4 color;
        public AiAttackOnCollide<Player> playerAttackAI;
        public AiPursue<Player> playerPursueAI;

        public AiSearch<Player> playerSearchAI;

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
            color = new Color4((float) r.NextDouble(), (float) r.NextDouble(), (float) r.NextDouble(), 1);
            SetCustomRenderer(new SlimeRenderer());
            AaBb = new AxisAlignedBb(-0.25f, 0, 0.25f, 0.5f);

            playerSearchAI = new AiSearch<Player>(7);
            playerPursueAI = new AiPursue<Player>(playerSearchAI, 0.1f);
            playerAttackAI = new AiAttackOnCollide<Player>(AttackValue);
        }

        public override string GetName()
        {
            return "entity.living.slime";
        }

        //public override Mov
        public override void Update()
        {
            playerSearchAI.Update(this);
            playerPursueAI.Update(this);

            base.Update();
        }

        public override void OnCollideWith(Entity _obj)
        {
            playerAttackAI.OnCollision(this, _obj);
        }
    }
}
﻿using GlLib.Client.Api.Renderers;
using GlLib.Common.Entities.Intelligence;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities
{
    internal class Bat : EntityLiving
    {
        public AiAttackOnCollide<Player> playerAttackAi;
        public AiPursue<Player> playerPursueAi;

        public AiSearch<Player> playerSearchAi;

        public Bat()
        {
            Initialize();
        }

        public Bat(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            Initialize();
        }

        private Player Target { get; set; }

        private void Initialize()
        {
            var renderer = new SimpleAttackingLivingRenderer("bat/bat");
            renderer.customize += (_e, _r) =>
            {
                ((SimpleAttackingLivingRenderer) _r).sprites[EntityState.DirectedAttack].step = 3;
            };
            SetCustomRenderer(renderer);
            AaBb = new AxisAlignedBb(-0.2f, -0.2f, 0.2f, 0.2f);

            playerSearchAi = new AiSearch<Player>(7);
            playerPursueAi = new AiPursue<Player>(playerSearchAi);
            playerAttackAi = new AiAttackOnCollide<Player>(5);
        }

        public override string GetName()
        {
            return "entity.living.bat";
        }

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
using System.Linq;
using GlLib.Client.Api.Renderers;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Entities.Intelligence;
using GlLib.Common.Map;
using GlLib.Utils;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities
{
    internal class Bat : EntityLiving
    {

        public Bat()
        {
            Initialize();
        }

        public Bat(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            Initialize();
        }

        private Player Target { get; set; }

        public AiSearch<Player> playerSearchAi;
        public AiPursue<Player> playerPursueAi;
        public AiAttackOnCollide<Player> playerAttackAi;

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
            playerPursueAi = new AiPursue<Player>(playerSearchAi, 0.2f);
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
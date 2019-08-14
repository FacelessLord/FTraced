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

        public AISearch<Player> playerSearchAI;
        public AIPursue<Player> playerPursueAI;
        public AIAttackOnCollide<Player> playerAttackAI;

        private void Initialize()
        {
            var renderer = new SimpleAttackingLivingRenderer("bat/bat");
            renderer.customize += (_e, _r) =>
            {
                ((SimpleAttackingLivingRenderer) _r).sprites[EntityState.DirectedAttack].step = 3;
            };
            SetCustomRenderer(renderer);
            AaBb = new AxisAlignedBb(-0.2, -0.2, 0.2, 0.2);

            playerSearchAI = new AISearch<Player>(7);
            playerPursueAI = new AIPursue<Player>(playerSearchAI);
            playerAttackAI = new AIAttackOnCollide<Player>(5);
        }

        public override string GetName()
        {
            return "entity.living.bat";
        }

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
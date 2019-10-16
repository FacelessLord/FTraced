using GlLib.Client.Api.Renderers;
using GlLib.Common.Entities.Intelligence;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities
{
    internal class EntityBat : EntityLiving
    {
        public AiAttackOnCollide<EntityPlayer> playerAttackAi;
        public AiPursue<EntityPlayer> playerPursueAi;

        public AiSearch<EntityPlayer> playerSearchAi;

        public EntityBat()
        {
            Initialize();
        }

        public EntityBat(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            Initialize();
        }

        private EntityPlayer Target { get; set; }

        private void Initialize()
        {
            var renderer = new SimpleAttackingLivingRenderer("bat/bat");
            renderer.customize += (_e, _r) =>
            {
                ((SimpleAttackingLivingRenderer) _r).sprites[EntityState.DirectedAttack].step = 3;
            };
            SetCustomRenderer(renderer);
            AaBb = new AxisAlignedBb(-0.2f, -0.2f, 0.2f, 0.2f);

            playerSearchAi = new AiSearch<EntityPlayer>(7);
            playerPursueAi = new AiPursue<EntityPlayer>(playerSearchAi);
            playerAttackAi = new AiAttackOnCollide<EntityPlayer>(5);
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
using System.Collections.Generic;
using System.Linq;
using GlLib.Common.Io;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities
{
    internal class SpawnBox : Entity
    {
        // Don't know how does it work ... 
        private const ushort LivingTime = 250;
        public List<EntityLiving> entitiesSpawnList;


        public SpawnBox()
        {
            Initialize();
            velocity = PlanarVector.Null;
        }

        private void Initialize()
        {
        }

        public override void Update()
        {
            base.Update();


   
            if (Proxy.GetServer().InternalTicks % LivingTime == 0 &&
                worldObj.GetEntitiesWithinAaBb(Position.ExpandBothTo(AaBb.Width,
                        AaBb.Height)).Count == 1 )
                worldObj.SpawnEntity(new EntitySlime(worldObj, Position));
        }

        public override string GetName()
        {
            return "entity.spawnbox";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    internal class SpawnBox : Entity
    {
        private const ushort RespawnTime = 1;
        private Entity[] SpawnEntities;

        public SpawnBox()
        {
            AaBb = new AxisAlignedBb(Position.ToPlanarVector(),
                new PlanarVector(2,2));
            SpawnEntities = new []{new EntitySlime(worldObj,Position),
            new EntitySlime(worldObj,Position),
            new EntitySlime(worldObj,Position)};
        }

        public override void Update()
        {
            base.Update();


            if (Proxy.GetServer().MachineTime.Second % RespawnTime == 0 &&
                worldObj.GetEntitiesWithinAaBb(AaBb).Count == 0)
            {
                for (int i = 0; i < SpawnEntities.Length; ++i)
                    worldObj.SpawnEntity(SpawnEntities[i]);   
            }
        }

        public override string GetName()
        {
            return "entity.spawnbox";
        }
    }
}

using System.Collections.Generic;
using GlLib.Common.Map;

namespace GlLib.Utils
{
    public class PlayerData
    {
        public World _world;
        public RestrictedVector3D _position;

        public PlayerData(World world,RestrictedVector3D position)
        {
            (_world, _position) = (world, position);
        }
        
        public void SaveToNbt(NbtTag tag)
        {
            //todo
        }

        public void LoadFromNbt(NbtTag tag)
        {
            //todo
        }
    }
}
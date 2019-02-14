using System.Collections.Generic;
using GlLib.Common.Map;
using GlLib.Server;

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
            tag.SetInt("WorldId",_world._worldId);
            tag.SetString("WorldId",_position.ToString());
        }

        public void LoadFromNbt(NbtTag tag)
        {
            _world = ServerInstance.GetWorldById(tag.GetInt("WorldId"));
            _position = RestrictedVector3D.FromString(tag.GetString("WorldId"));
        }
    }
}
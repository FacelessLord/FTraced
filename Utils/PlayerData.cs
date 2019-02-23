using GlLib.Common;
using GlLib.Common.Map;
using GlLib.Server;

namespace GlLib.Utils
{
    public class PlayerData
    {
        public string _nickname;
        public World _world;
        public RestrictedVector3D _position;

        public PlayerData(World world, RestrictedVector3D position, string nickname)
        {
            (_world, _position, _nickname) = (world, position, nickname);
        }

        public void SaveToNbt(NbtTag tag)
        {
            tag.SetInt("WorldId", _world._worldId);
            tag.SetString("Position", _position.ToString());
            tag.SetString("Nickname", _nickname);
        }

        public static PlayerData LoadFromNbt(NbtTag tag)
        {
            World world = Proxy.GetServer().GetWorldById(tag.GetInt("WorldId"));
            RestrictedVector3D position = RestrictedVector3D.FromString(tag.GetString("Position"));
            string nickname = tag.GetString("Nickname");
            return new PlayerData(world, position, nickname);
        }
    }
}
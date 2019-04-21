namespace GlLib.Utils
{
    public class PlayerData
    {
        public string nickname;
        public RestrictedVector3D position;
        public int worldId;

        public PlayerData(int _worldId, RestrictedVector3D _position, string _nickname)
        {
            (this.worldId, this.position, this.nickname) = (worldId: _worldId, position: _position, nickname: _nickname);
        }

        public void SaveToNbt(NbtTag _tag)
        {
            _tag.SetInt("WorldId", worldId);
            _tag.SetString("Position", position.ToString());
            _tag.SetString("Nickname", nickname);
        }

        public static PlayerData LoadFromNbt(NbtTag _tag)
        {
            var worldId = _tag.GetInt("WorldId");
            var position = RestrictedVector3D.FromString(_tag.GetString("Position"));
            var nickname = _tag.GetString("Nickname");
            return new PlayerData(worldId, position, nickname);
        }
    }
}
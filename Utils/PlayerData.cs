namespace GlLib.Utils
{
    public class PlayerData
    {
        public string nickname;
        public RestrictedVector3D position;
        public int worldId;

        public PlayerData(int worldId, RestrictedVector3D position, string nickname)
        {
            (this.worldId, this.position, this.nickname) = (worldId, position, nickname);
        }

        public void SaveToNbt(NbtTag tag)
        {
            tag.SetInt("WorldId", worldId);
            tag.SetString("Position", position.ToString());
            tag.SetString("Nickname", nickname);
        }

        public static PlayerData LoadFromNbt(NbtTag tag)
        {
            var worldId = tag.GetInt("WorldId");
            var position = RestrictedVector3D.FromString(tag.GetString("Position"));
            var nickname = tag.GetString("Nickname");
            return new PlayerData(worldId, position, nickname);
        }
    }
}
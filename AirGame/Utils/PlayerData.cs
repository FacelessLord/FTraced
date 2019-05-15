using GlLib.Common.Map;

namespace GlLib.Utils
{
    public class PlayerData
    {
        public ushort CastLevel { get; }
        public World world;
            

        public PlayerData()
        {
            CastLevel = 100;
        }

        public void SaveToNbt(NbtTag _tag)
        {
        }

        public static PlayerData LoadFromNbt(NbtTag _tag)
        {
            return new PlayerData();
        }
    }
}
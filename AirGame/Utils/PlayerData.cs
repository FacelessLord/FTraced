namespace GlLib.Utils
{
    public class PlayerData
    {
        public void SaveToNbt(NbtTag _tag)
        {
        }

        public static PlayerData LoadFromNbt(NbtTag _tag)
        {
            return new PlayerData();
        }
    }
}
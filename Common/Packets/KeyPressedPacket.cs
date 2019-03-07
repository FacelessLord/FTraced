using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Common.Packets
{
    public class KeyPressedPacket : Packet
    {
        public Key key;

        public KeyPressedPacket()
        {
            key = Key.Unknown;
        }

        public KeyPressedPacket(Key key)
        {
            this.key = key;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetInt("Key", (int) key);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            key = (Key) tag.GetInt("Key");
        }
    }
}
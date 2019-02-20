using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Common.Packets
{
    public class KeyPressedPacket : Packet
    {
        public Key _key;
        
        public KeyPressedPacket(Key key)
        {
            _key = key;
        }
        
        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetInt("Key",(int)_key);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            _key = (Key)tag.GetInt("Key");
        }
    }
}
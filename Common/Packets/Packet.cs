using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    /// <summary>
    /// You can store any information you need here,
    /// but implement methods to save and load info from nbt tag.
    /// Otherwise your information wouldn't be sent
    ///
    /// You must also implement empty constructor
    /// </summary>
    public abstract class Packet
    {
        public static int _typeId = -1;
        
        /// <summary>
        /// Prepare information to be sent to receiver
        /// </summary>
        /// <param name="tag">Tag to save info</param>
        public abstract void WriteToNbt(NbtTag tag);

        /// <summary>
        /// Read received information
        /// </summary>
        /// <param name="tag">Tag to load info</param>
        public abstract void ReadFromNbt(NbtTag tag);

        public void OnClientReceive(ClientService client)
        {

        }

        public void OnServerReceive()
        {

        }
    }
}
using GlLib.Common.Packets;
using GlLib.Utils;

namespace GlLib.Common
{
    public class Proxy
    {
        public void SendPacketToPlayer(string nickName, Packet packet)
        {
            //todo
        }
        
        public void SendPacketToAllAround(PlanarVector pos,double range, Packet packet)
        {
            //todo
        }
        
        public void SendPacketToWorld(int worldId, Packet packet)
        {
            //todo
        }
        
        public void SendPacketToServer(Packet packet)
        {
            //todo
        }
        
        public void AwaitForPacketFromServer(int packetType)
        {
            //todo
        }
    }
}
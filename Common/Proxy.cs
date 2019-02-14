using System.Collections.Generic;
using GlLib.Client;
using GlLib.Common.Packets;
using GlLib.Utils;
using System.Linq;
using GlLib.Common.Map;
using GlLib.Server;

namespace GlLib.Common
{
    public class Proxy
    {
        public static List<ClientService> _clients = new List<ClientService>();

        public static void SendPacketToPlayer(string nickName, Packet packet)
        {
            if (Config._isIntegratedServer)
            {
                _clients.Where(c => c._nickName == nickName).ToList().ForEach(c => c.HandlePackage(packet));
            }

            //todo not Integrated Server
        }

        public static void SendPacketToAllAround(PlanarVector pos, double range, Packet packet)
        {

            //todo not Integrated Server
        }

        public static void SendPacketToWorld(int worldId, Packet packet)
        {
            //todo not Integrated Server
        }

        public static void SendPacketToServer(Packet packet)
        {
            if (Config._isIntegratedServer)
            {
                ServerInstance.HandlePackage(packet);
            }

            //todo not Integrated Server
        }
        
        public static List<int> _awaitedPacketIds = new List<int>();

        public static Packet _awaitedPacket = null;
        public static Packet AwaitForPacketFromServer(int packetId)
        {//todo think of usability
            _awaitedPacketIds.Add(packetId);
            while (_awaitedPacket == null)
            {
                
            }
            
            return _awaitedPacket;
        }

        public static void Sync()
        {
            //todo send SyncPackage
        }
    }
}
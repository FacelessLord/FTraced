using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GlLib.Client
{
    public class GameClient
    {

        public GameClient()
        {
            
        }

        public void MakeConnection(IPAddress _address, Int32 _port)
        {
            var client = new TcpClient();
            client.Connect(_address, _port);
            client.GetStream().Write(Encoding.Unicode.GetBytes("Check_"));
        }

        public void Disconnect()
        {
            
        }
    }
}
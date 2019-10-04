using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GlLib.Common;

namespace GlLib.Client
{
    public class GameClient
    {
        public ClientService service;
        public Thread serviceThread;
        public GameClient()
        {
            
        }
        
        public void Start()
        {
            StartService();
        }

        public void StartService()
        {
            service = new ClientService(Config.playerName, Config.playerPassword);
            serviceThread = new Thread(() =>
            {
                service.Start();
                service.Loop();
                service.Exit();
            }) {Name = Side.Client.ToString()};
            serviceThread.Start();
            Proxy.AwaitWhile(() => service.profiler.state < State.Loop);
        }

        public void MakeConnection(IPAddress _address, Int32 _port)
        {
            var client = new TcpClient();
            client.Connect(_address, _port);
            client.GetStream().Write(new[] {(byte) service.nickName.Length, (byte) service.password.Length});
            client.GetStream().Write(GetBytes(service.nickName));
            client.GetStream().Write(GetBytes(service.password));
        }

        public static byte[] GetBytes(string _msg)
        {
            return Encoding.Unicode.GetBytes(_msg);
        }

        public void Disconnect()
        {
            
        }
    }
}
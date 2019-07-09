using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using GlLib.Common;
using GlLib.Common.Entities;

namespace GlLib.Server
{
    public static class GameServer
    {
        /// <summary>
        /// TCPListener instance
        /// </summary>
        private static TcpListener _server;

        /// <summary>
        /// Addreess of this Server 
        /// </summary>
        private static IPAddress _address;

        /// <summary>
        /// The port the connection will be running through 
        /// </summary>
        private static int _port = 25560;

        /// <summary>
        /// Loads Server Net side
        /// Starts writing Server_net.log
        /// </summary>
        public static void Start()
        {
            _address = GetAddresses().First();
            _server = new TcpListener(_address, _port);
            _server.Start();
            
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data;

            while (true)
            {
//                    Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                // Waiting for connection
                TcpClient client = _server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    //RECEIVED MESSAGE
                    // Translate data bytes to a ASCII string.
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    
//                    Console.WriteLine("Received: {0}", data);

                    //RESPONSE
                    // Process the data sent by the client.
//                    data = data.ToUpper();
//
//                    byte[] msg = Encoding.ASCII.GetBytes(data);
//
//                    // Send back a response.
//                    stream.Write(msg, 0, msg.Length);
//                    Console.WriteLine("Sent: {0}", data);
                }

                // Shutdown and end connection
//                client.Close();
            }
        }

        public static IEnumerable<IPAddress> GetAddresses()
        {
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        yield return ip.Address.MapToIPv4();
                    }
                }
            }
        }


        /// <summary>
        /// Sends Packet to given player
        /// maybe returning response
        /// </summary>
        /// <param name="_pkt">Packet to send</param>
        /// <param name="_player">Target</param>
        /// <remarks>
        /// Used to notify players
        /// </remarks>
        public static void SendPacketTo(Packet _pkt, Player _player)
        {

        }

        /// <summary>
        /// Sends Packet to all players connected to the server
        /// </summary>
        /// <param name="_pkt">Packet to send</param>
        /// <remarks>
        /// Used in synchronization
        /// </remarks>
        /// <exception cref="NotImplementedException"></exception>
        public static void SendPacketToAll(Packet _pkt)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}
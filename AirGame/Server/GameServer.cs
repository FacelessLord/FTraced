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

        public static bool isOpened;

        /// <summary>
        /// Loads Server Net side
        /// Starts writing Server_net.log
        /// </summary>
        public static async void Start()
        {
            _address = GetAddresses().First();
            _server = new TcpListener(_address, _port);
            _server.Start();
            
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data;
            isOpened = true;
            while (isOpened)
            {
                TcpClient client = await _server.AcceptTcpClientAsync().ConfigureAwait(false);//non blocking waiting                    
                // We are already in the new task to handle this client...   
                RegisterClient(client);
                HandleClient(client);
            }
        }

        /// <summary>
        /// Contains data-Token and corresponding TcpClient for player 
        /// </summary>
        public static Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        /// <summary>
        /// Contains data-Token and corresponding PlayerData entry
        /// </summary>
        public static Dictionary<string, PlayerDataEntry> dataEntries = new Dictionary<string, PlayerDataEntry>();//TODO auth token usage
        
        /// <summary>
        /// Registers client on the server
        /// Sends PlayerData and World data
        /// </summary>
        /// <param name="_client">Client to register</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void RegisterClient(TcpClient _client)
        {
            var stream = _client.GetStream();
            byte nicknameLength = ReadFromStream(stream,1)[0];
            byte passwordLength = ReadFromStream(stream,1)[0];
            byte[] nicknameBytes = ReadFromStream(stream, nicknameLength);
            byte[] passwordBytes = ReadFromStream(stream, passwordLength);

            string nickname = Encoding.Unicode.GetString(nicknameBytes, 0, nicknameLength);
            string password = Encoding.Unicode.GetString(passwordBytes, 0, passwordLength);
            
            //TODO Create data token
            
            //TODO Send PlayerData
            //TODO Send World
        }

        /// <summary>
        /// Performs all lifecycle handling of client
        /// </summary>
        /// <param name="_client">Client to handle</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void HandleClient(TcpClient _client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Wrapper method used to read known count of bytes from stream
        /// </summary>
        /// <param name="_stream">Stream to read from</param>
        /// <param name="_size">Count of bytes to read</param>
        /// <returns>Bytes read from stream</returns>
        private static byte[] ReadFromStream(NetworkStream _stream, int _size)
        {
            var buffer = new byte[_size];
            _stream.Read(buffer, 0, _size);
            return buffer;
        }

        /// <summary>
        /// Performs searching through all net devices on current machine
        /// </summary>
        /// <returns>Enumeration of all addresses of current machine</returns>
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
        /// TODO maybe returning response
        /// </summary>
        /// <param name="_pkt">Packet to send</param>
        /// <param name="_player">Target</param>
        /// <remarks>
        /// Used to notify players
        /// </remarks>
        public static void SendPacketTo(Packet _pkt, Player _player)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}
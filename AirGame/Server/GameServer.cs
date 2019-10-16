using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Io;

namespace GlLib.Server
{
    public class GameServer
    {
        public static Thread serviceThread;

        /// <summary>
        ///     Addreess of this Server
        /// </summary>
        private IPAddress _address;

        /// <summary>
        ///     The port the connection will be running through
        /// </summary>
        private readonly int _port = 25560;

        /// <summary>
        ///     TCPListener instance
        /// </summary>
        private TcpListener _server;

        /// <summary>
        ///     Contains data-Token and corresponding TcpClient for player
        /// </summary>
        public Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        /// <summary>
        ///     Contains data-Token and corresponding PlayerData entry
        /// </summary>
        public Dictionary<string, PlayerDataEntry>
            dataEntries = new Dictionary<string, PlayerDataEntry>(); //TODO auth token usage

        public bool isOpened;

        /// <summary>
        ///     Loads Server Net side
        ///     Starts writing Server_net.log
        /// </summary>
        public async void Start()
        {
            StartService();
            
            //_address = GetAddresses().First();
            //_server = new TcpListener(_address, _port);
            //_server.Start();
            
            //// Buffer for reading data
            //Byte[] bytes = new Byte[256];
            //String data;
            //isOpened = true;
            //while (isOpened)
            //{
            //    TcpClient client = await _server.AcceptTcpClientAsync().ConfigureAwait(false);//non blocking waiting                    
            //    // We are already in the new task to handle this client...   
            //    RegisterClient(client);
            //    HandleClient(client);
            //}
        }

        public void StartService()
        {
            var server = new ServerInstance();
            serviceThread = new Thread(() =>
            {
                server.Start();
                server.Loop();
                server.Exit();
            }) {Name = Side.Server.ToString()};
            serviceThread.Start();
            Proxy.AwaitWhile(() => server.profiler.state < State.Loop);
        }

        /// <summary>
        ///     Registers client on the server
        ///     Sends PlayerData and World data
        /// </summary>
        /// <param name="_client">Client to register</param>
        /// <exception cref="NotImplementedException"></exception>
        private void RegisterClient(TcpClient _client)
        {
            var stream = _client.GetStream();
            var nicknameLength = ReadFromStream(stream, 1)[0];
            var passwordLength = ReadFromStream(stream, 1)[0];
            var nicknameBytes = ReadFromStream(stream, nicknameLength);
            var passwordBytes = ReadFromStream(stream, passwordLength);

            var nickname = Encoding.Unicode.GetString(nicknameBytes, 0, nicknameLength);
            var password = Encoding.Unicode.GetString(passwordBytes, 0, passwordLength);

            SidedConsole.Write(nickname + " : " + password);

            //TODO Create data token

            //TODO Send PlayerData
            //TODO Send World = world.Serialize -> Client
        }

        /// <summary>
        ///     Performs all lifecycle handling of client
        /// </summary>
        /// <param name="_client">Client to handle</param>
        /// <exception cref="NotImplementedException"></exception>
        private void HandleClient(TcpClient _client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Handles received packet
        /// </summary>
        /// <param name="_client">Client the packet</param>
        /// <param name="_packet">Packet to handle</param>
        /// <exception cref="NotImplementedException"></exception>
        private void HandlePacket(TcpClient _client, Packet _packet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Wrapper method used to read known count of bytes from stream
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
        ///     Performs searching through all net devices on current machine
        /// </summary>
        /// <returns>Enumeration of all addresses of current machine</returns>
        public static IEnumerable<IPAddress> GetAddresses()
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
                if (item.OperationalStatus == OperationalStatus.Up)
                    foreach (var ip in item.GetIPProperties().UnicastAddresses)
                        yield return ip.Address.MapToIPv4();
        }


        /// <summary>
        ///     Sends Packet to given player
        ///     TODO maybe returning response
        /// </summary>
        /// <param name="_pkt">Packet to send</param>
        /// <param name="_entityPlayer">Target</param>
        /// <remarks>
        ///     Used to notify players
        /// </remarks>
        public void SendPacketTo(Packet _pkt, EntityPlayer _entityPlayer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Sends Packet to all players connected to the server
        /// </summary>
        /// <param name="_pkt">Packet to send</param>
        /// <remarks>
        ///     Used in synchronization
        /// </remarks>
        /// <exception cref="NotImplementedException"></exception>
        public void SendPacketToAll(Packet _pkt)
        {
            throw new NotImplementedException();
        }
    }
}
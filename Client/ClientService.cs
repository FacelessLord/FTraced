using System.Net;
using GlLib.Common.Entities;

namespace GlLib.Client
{
    public class ClientService
    {
        public IPAddress _ip;
        public string _nickName;
        public int _passwordHashcode;
        public Player _player;

        public ClientService(IPAddress ip, string nickName, string password)
        {
            _ip = ip;
            _nickName = nickName;
            _passwordHashcode = password.GetHashCode();
        }

        public bool ConnectToServer(IPAddress ip)
        {
            //todo
            return false;
        }
    }
}
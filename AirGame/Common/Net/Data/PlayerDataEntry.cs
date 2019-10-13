using System;

namespace GlLib.Common
{
    public class PlayerDataEntry
    {
        /// <summary>
        ///     Encrypted player password
        /// </summary>
        public string password; //TODO Encryption

        /// <summary>
        ///     Player username
        /// </summary>
        public string username;

        /// <summary>
        ///     Player UUID that can't be changed
        /// </summary>
        public Guid uuid;

        public PlayerDataEntry(string _username, string _password)
        {
            username = _username;
            password = _password;
            //todo uuid
        }

        public PlayerDataEntry(Guid _uuid, string _password)
        {
            uuid = _uuid;
            password = _password;

            //todo username
        }
    }
}
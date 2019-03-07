using GlLib.Client;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public abstract class Request : Packet
    {
        protected string password;
        protected string playerNickname;

        public Request()
        {

        }

        public Request(string nickname, string password)
        {
            playerNickname = nickname;
            this.password = password;
        }

        public Request(ClientService client)
        {
            password = client.password;
            playerNickname = client.nickName;
        }

        public override void WriteToNbt(NbtTag tag)
        {
            tag.SetString("Nickname", playerNickname);
            tag.SetString("Password", password);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            playerNickname = tag.GetString("Nickname");
            password = tag.GetString("Password");
        }
    }
}
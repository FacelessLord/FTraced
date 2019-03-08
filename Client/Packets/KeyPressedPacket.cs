using System.Linq;
using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Common.Packets
{
    public class KeyPressedPacket : Request
    {
        public string command;

        public KeyPressedPacket()
        {
            command = "null";
        }

        public KeyPressedPacket(ClientService client, Key key) : base(client)
        {
            command = Proxy.GetClient().binds.GetCommand(key);
        }

        public override void WriteToNbt(NbtTag tag)
        {
            base.WriteToNbt(tag);
            tag.SetString("Command", command);
        }

        public override void ReadFromNbt(NbtTag tag)
        {
            base.ReadFromNbt(tag);
            command = tag.GetString("Command");
        }

        public override bool RequiresReceiveMessage()
        {
            return true;
        }

        public override void OnServerReceive(ServerInstance server)
        {
            base.OnServerReceive(server);
            server.clients.Where(c => c.nickName == playerNickname).ToList()
                .ForEach(c => c.player.usedBinds.Add(command));
            //todo test for multiple similar commands like: up,up => up
        }
    }
}
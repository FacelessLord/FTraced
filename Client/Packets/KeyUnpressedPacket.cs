using System.Linq;
using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Common.Packets
{
    public class KeyUnpressedPacket : Request
    {
        public string command;

        public KeyUnpressedPacket()
        {
            command = "null";
        }

        public KeyUnpressedPacket(ClientService client, Key key) : base(client)
        {
            command = Proxy.GetClient().binds.GetCommand(key);
        }
        
        public override bool RequiresReceiveMessage()
        {
            return false;
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

        public override void OnServerReceive(ServerInstance server)
        {
            base.OnServerReceive(server);
            server.clients.Where(c => c.nickName == playerNickname).ToList()
                .ForEach(c => c.player.usedBinds.Remove(command));
            //todo test for removing commands that are not int Set
        }
    }
}
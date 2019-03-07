using System.Threading;
using GlLib.Client;
using GlLib.Client.Input;
using GlLib.Common.Map;
using GlLib.Common.Packets;
using GlLib.Common.Registries;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common
{
    public abstract class SideService
    {
        public const int FrameTime = 50;
        public Blocks blocks;
        public EntityRegistry entities;
        public PacketHandler packetHandler;
        public PacketRegistry packets;

        public GameRegistry registry;

        public int serverId;
        public Side side;
        public State state = State.Off;

        public SideService(Side side)
        {
            this.side = side;
            registry = new GameRegistry();
            blocks = new Blocks(registry);
            entities = new EntityRegistry(registry);
            packets = new PacketRegistry();
            switch (side)
            {
                case Side.Client:
                    packetHandler = new ClientPacketHandler(this);
                    break;
                case Side.Server:
                    packetHandler = new ServerPacketHandler(this);
                    break;
            }
        }

        public void Start()
        {
            if (side == Side.Client)
                KeyBinds.Register();
            blocks.Register();
            entities.Register();
            packetHandler.StartPacketHandler();
            packets.Register();
            Proxy.RegisterService(this);
            OnStart();
        }

        public void Loop()
        {
            state = State.Loop;
            while (true)
            {
                OnServiceUpdate();
                Thread.Sleep(FrameTime);
            }
        }

        public void Exit()
        {
            state = State.Exiting;
            OnExit();
            state = State.Off;
        }

        public abstract void OnServiceUpdate();

        public abstract void OnExit();
        public abstract void OnStart();

        public virtual void HandlePacket(Packet packet)
        {
            if (packet.RequiresReceiveMessage())
                SidedConsole.WriteLine($"Packet {packets.GetPacketId(packet)} has been received.");
            if (side == Side.Server)
                packet.OnServerReceive((ServerInstance) this);
            if (side == Side.Client)
                packet.OnClientReceive((ClientService) this);
        }
    }
}
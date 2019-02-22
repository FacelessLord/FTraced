using System.Threading;
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
        public PacketHandler _packetHandler;
        public const int FrameTime = 50;
        
        public int _serverId;
        public Side _side;
        public State _state = State.Off;

        public SideService(Side side)
        {
            _side = side;
            _registry = new GameRegistry();
            _blocks = new Blocks(_registry);
            _entities = new EntityRegistry(_registry);
            _packets = new PacketRegistry();
            switch (side)
            {
                case Side.Client : 
                _packetHandler = new ClientPacketHandler(this);
                break;
            }
        }

        public GameRegistry _registry;
        public Blocks _blocks;
        public EntityRegistry _entities;
        public PacketRegistry _packets;

        public void Start()
        {
            if(_side == Side.Client)
                KeyBinds.Register();
            _blocks.Register();
            _entities.Register();
            _packetHandler.StartPacketHandler();
            _packets.Register();
            OnStart();
        }

        public void Loop()
        {
            _state = State.Loop;
            while (true)
            {
                OnServiceUpdate();
                Thread.Sleep(FrameTime);
            }
        }
        
        public void Exit()
        {
            _state = State.Exiting;
            OnExit();
            _state = State.Off;
        }

        public abstract void OnServiceUpdate();

        public abstract void OnExit();
        public abstract void OnStart();
        
        public virtual void HandlePacket(Packet packet)
        {
            if(packet.RequiresReceiveMessage())
                SidedConsole.WriteLine($"Packet {_packets.GetPacketId(packet)} has been received.");
            if(_side == Side.Server)
                packet.OnServerReceive(this);
            if(_side == Side.Client)
                packet.OnClientReceive(this);
        }
        
    }
}
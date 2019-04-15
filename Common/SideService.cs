using System.Threading;
using GlLib.Client;
using GlLib.Common.Map;
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

        public GameRegistry registry;

        public int serverId;
        public Side side;
        public State state = State.Off;

        public SideService(Side _side)
        {
            this.side = _side;
            registry = new GameRegistry();
            blocks = new Blocks(registry);
            entities = new EntityRegistry(registry);
        }

        public void Start()
        {
            blocks.Register();
            entities.Register();
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

    }
}
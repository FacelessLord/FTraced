using System.Threading;
using GlLib.Common.Map;
using GlLib.Common.Registries;

namespace GlLib.Common
{
    public abstract class SideService
    {
        public const int FrameTime = 50;
        public Blocks blocks;
        public EntityRegistry entities;
        public ItemRegistry items;

        public GameRegistry registry;
        public Profiler profiler = new Profiler();

        public int serverId;
        public Side side;

        public SideService(Side _side)
        {
            side = _side;
            registry = new GameRegistry();
            blocks = new Blocks(registry);
            entities = new EntityRegistry(registry);
            items = new ItemRegistry(registry);
        }

        public void Start()
        {
            blocks.Register();
            entities.Register();
            items.Register();
            Proxy.RegisterService(this);
            OnStart();
        }

        public void Loop()
        {
            profiler.SetState(State.Loop);
            while (!Proxy.Exit)
            {
                OnServiceUpdate();
                Thread.Sleep(FrameTime);
            }
        }

        public void Exit()
        {
            profiler.SetState(State.Exiting);
            OnExit();
            profiler.SetState(State.Off);
        }

        public abstract void OnServiceUpdate();
        public abstract void OnExit();
        public abstract void OnStart();
    }
}
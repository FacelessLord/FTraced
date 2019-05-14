using System.Threading;
using GlLib.Common.Registries;
using GlLib.Utils;

namespace GlLib.Common
{
    public abstract class SideService
    {
        public const int FrameTime = 50;
        public Profiler profiler = new Profiler();

        public GameRegistry registry;

        public int serverId;
        public bool askedToStop = false;
        public Side side;

        public SideService(Side _side)
        {
            side = _side;
            registry = new GameRegistry();
        }

        public void Start()
        {
            profiler.SetState(State.Loading);
            profiler.SetState(State.LoadingRegistries);
            Proxy.RegisterService(this);
            registry.Load();
            OnStart();
        }

        public void Loop()
        {
            profiler.SetState(State.Loop);
            while (!Proxy.Exit && !askedToStop)
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

        public void AskToStop(string _cause)
        {
            askedToStop = true;
            SidedConsole.WriteLine("Asked to stop. Preparing to stop.");
        }

        public abstract void OnServiceUpdate();
        public abstract void OnExit();
        public abstract void OnStart();
    }
}
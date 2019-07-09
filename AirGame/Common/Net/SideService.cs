using System;
using System.Diagnostics;
using System.Threading;
using GlLib.Common.Io;
using GlLib.Common.Registries;
using GlLib.Utils;

namespace GlLib.Common
{
    public abstract class SideService
    {
        public const int FrameTime = 50;

        /// <summary>
        /// boolean that indicates whether service should stop
        /// </summary>
        public bool askedToStop;
        /// <summary>
        /// Service Profiler
        /// </summary>
        public Profiler profiler = new Profiler();

        /// <summary>
        /// Registry of all ingame materials 
        /// </summary>
        public GameRegistry registry;

        /// <summary>
        /// Side the service is working on
        /// </summary>
        public Side side;

        protected SideService(Side _side)
        {
            side = _side;
            registry = new GameRegistry();
        }

        public long InternalTicks =>
            (DateTime.UtcNow - Process.GetCurrentProcess()
                 .StartTime
                 .ToUniversalTime())
            .Ticks;

        public DateTime MachineTime =>
            DateTime.UtcNow
                .ToUniversalTime();

        public double InternalMilliseconds =>
            (DateTime.UtcNow - Process.GetCurrentProcess()
                 .StartTime
                 .ToUniversalTime())
            .TotalMilliseconds;

        /// <summary>
        /// Loads Game Service
        /// </summary>
        public void Start()
        {
            profiler.SetState(State.Loading);
            profiler.SetState(State.LoadingRegistries);
            Proxy.RegisterService(this);
            registry.Load();
            OnStart();
        }

        /// <summary>
        /// Performs Tick Logic
        /// </summary>
        public void Loop()
        {
            profiler.SetState(State.Loop);
            while (!Proxy.Exit && !askedToStop)
            {
                OnServiceUpdate();
                Thread.Sleep(FrameTime);
            }
        }

        /// <summary>
        /// Wrapper method to stop the game
        /// </summary>
        public void Exit()
        {
            profiler.SetState(State.Exiting);
            OnExit();
            profiler.SetState(State.Off);
        }

        /// <summary>
        /// Wrapper method to stop game service
        /// </summary>
        /// <param name="_cause"></param>
        public void AskToStop(string _cause)
        {
            askedToStop = true;
            SidedConsole.WriteLine("Asked to stop." + _cause + ". Preparing to stop.");
        }

        /// <summary>
        /// Event raised every update tick
        /// </summary>
        public abstract void OnServiceUpdate();
        
        /// <summary>
        /// Event raised every update tick
        /// </summary>
        public abstract void OnExit();
        
        /// <summary>
        /// Event raised when service loads
        /// </summary>
        public abstract void OnStart();
    }
}
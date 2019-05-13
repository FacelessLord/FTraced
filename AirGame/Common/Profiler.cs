using System;
using GlLib.Utils;

namespace GlLib.Common
{
    public class Profiler
    {
        public State state = State.Off;
        public int maxOperationCount = 1;
        public int operationCount = 0;

        public void SetState(State _state, int _maxOperationCount = -1)
        {
//            SidedConsole.WriteLine("State Changed: " + _state);
            state = _state;
            maxOperationCount = _maxOperationCount;
            operationCount = 0;
        }

        public double GetPrecentage()
        {
            return (operationCount / (double) maxOperationCount);
        }

        public void UpdateCounter()
        {
            operationCount++;
            if (operationCount > maxOperationCount && maxOperationCount != -1)
            {
                throw new IndexOutOfRangeException(
                    "You've done more operations then you planned.\n" +
                    "It is probably the result of bad code structure or lack of a part of code");
            }
        }
    }

    public enum State
    {
        Off,
        CoreStarting,
        MainMenu,
        LoadingRegistries,
        LoadingWorld,
        SavingWorld,
        Connection,
        Loading,
        Loop,
        Exiting
    }
}
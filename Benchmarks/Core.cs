using System;
using BenchmarkDotNet.Running;
using Benchmarks.Common;

namespace Benchmarks
{
    class Core
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<EntityMarks>();
        }
    }
}

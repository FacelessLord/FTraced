using BenchmarkDotNet.Attributes;
using GlLib.Common.Entities.Items;
using GlLib.Common.Map;

namespace Benchmarks.Common
{
    [ClrJob(baseline: true), CoreJob, CoreRtJob]
    [RPlotExporter, RankColumn]
    [MemoryDiagnoser]
    public class EntityMarks
    {
        private World world = new World("World", 1, false);

        [Params(5, 50, 100, 200)]
        public int N { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            for (int i = 0; i < N; i++)
            {
                world.SpawnEntity(new Coin());
            }
        }

        [Benchmark]
        public void Update() => world.Update();
        

    }
}

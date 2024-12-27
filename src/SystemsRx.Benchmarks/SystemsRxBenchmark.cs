using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemsRx.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31, warmupCount: 1, invocationCount: 1, iterationCount: 10)]
    [MemoryDiagnoser]
    [MarkdownExporter]
    public abstract class SystemsRxBenchmark
    {
        protected SystemsRxBenchmark()
        {

        }

        [GlobalSetup]
        public abstract void Setup();

        [GlobalCleanup]
        public abstract void Cleanup();
    }
}
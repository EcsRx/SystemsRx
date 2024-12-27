using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using SystemsRx.Benchmarks.Benchmarks;
using SystemsRx.Extensions;

namespace SystemsRx.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmarks = new []
            {
                BenchmarkConverter.TypeToBenchmarks(typeof(IdPoolBenchmarks)),
                BenchmarkConverter.TypeToBenchmarks(typeof(OptimizedIdPoolBenchmarks)),
                BenchmarkConverter.TypeToBenchmarks(typeof(MultithreadedIdPoolBenchmarks)),
            };
            
            var summaries = BenchmarkRunner.Run(benchmarks);
            var consoleLogger = ConsoleLogger.Default;
            consoleLogger.Flush();
            summaries.ForEachRun(x =>
            {
                AsciiDocExporter.Default.ExportToLog(x, consoleLogger);
                consoleLogger.WriteLine();
            });
        }
    }
}
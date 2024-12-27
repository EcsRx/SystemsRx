﻿using BenchmarkDotNet.Attributes;
using SystemsRx.Pools;

namespace SystemsRx.Benchmarks.Benchmarks
{
    [BenchmarkCategory("Pools")]
    public class OptimizedIdPoolBenchmarks : SystemsRxBenchmark
    {
        [Params(1000, 10000, 100000)]
        public int PoolCount;

        public IIdPool IdPool { get; set; }
        public int[] IdList;

        public override void Setup()
        {
            IdPool = new IdPool(startingSize: PoolCount);
            IdList = new int[PoolCount];
        }

        public override void Cleanup()
        { }

        [Benchmark]
        public void OptimizedAllocationAndRelease()
        {
            for (var i = 0; i < PoolCount; i++)
            { IdList[i] = IdPool.AllocateInstance(); }
            
            for(var i = 0; i < PoolCount; i++)
            { IdPool.ReleaseInstance(IdList[i]); }
        }
    }
}
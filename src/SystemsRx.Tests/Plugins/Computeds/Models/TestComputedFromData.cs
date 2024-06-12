using System;
using SystemsRx.Computeds.Data;
using SystemsRx.MicroRx;
using SystemsRx.MicroRx.Subjects;

namespace SystemsRx.Tests.Plugins.Computeds.Models
{
    public class DummyData
    {
        public int Data { get; set; }
    }
    
    public class TestComputedFromData : ComputedFromData<int, DummyData>
    {
        public Subject<Unit> ManuallyRefresh = new Subject<Unit>();
        
        public TestComputedFromData(DummyData data) : base(data)
        {}

        public override IObservable<Unit> RefreshWhen()
        { return ManuallyRefresh; }

        public override int Transform(DummyData data)
        { return data.Data; }
    }
}
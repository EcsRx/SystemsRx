using System;
using SystemsRx.MicroRx;
using SystemsRx.MicroRx.Subjects;
using SystemsRx.Plugins.Computeds.Data;

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
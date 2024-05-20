using System;
using R3;

namespace SystemsRx.Scheduling
{
    public interface IUpdateScheduler : ITimeTracker, IDisposable
    {
        Observable<ElapsedTime> OnPreUpdate { get; }
        Observable<ElapsedTime> OnUpdate { get; }
        Observable<ElapsedTime> OnPostUpdate { get; }
    }
}
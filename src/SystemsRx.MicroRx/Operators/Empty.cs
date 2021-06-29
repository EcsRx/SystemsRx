using System;
using SystemsRx.MicroRx.Disposables;

namespace SystemsRx.MicroRx.Operators
{
    public class ImmutableEmptyObservable<T> : IObservable<T>
    {
        public static readonly ImmutableEmptyObservable<T> Instance = new ImmutableEmptyObservable<T>();

        private ImmutableEmptyObservable()
        {

        }
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
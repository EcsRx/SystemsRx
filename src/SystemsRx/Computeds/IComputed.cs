using System;

namespace SystemsRx.Computeds
{
    public interface IComputed<out T> : IObservable<T>
    {        
        T Value { get; }
    }
}
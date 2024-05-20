using System;

namespace SystemsRx.Computeds
{
    public interface IComputed<out T>
    {        
        T Value { get; }
    }
}
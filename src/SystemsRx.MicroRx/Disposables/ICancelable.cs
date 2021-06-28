using System;

namespace SystemsRx.MicroRx.Disposables
{
    public interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
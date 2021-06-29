using System;

namespace SystemsRx.MicroRx.Disposables
{
    public class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Singleton = new EmptyDisposable();

        private EmptyDisposable()
        {}

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
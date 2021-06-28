using System;

namespace SystemsRx.MicroRx.Disposables
{
    public class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Singleton = new EmptyDisposable();

        private EmptyDisposable()
        {

        }

        public void Dispose()
        {
        }
    }
}
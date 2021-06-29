using System;
using System.Threading;

namespace SystemsRx.MicroRx.Observers
{
    class StubbedSubscribeObserver<T> : IObserver<T>
    {
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        private int _isStopped = 0;

        public StubbedSubscribeObserver(Action<Exception> onError, Action onCompleted)
        {
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            // Stub doesnt do anything
        }

        public void OnError(Exception error)
        {
            if (Interlocked.Increment(ref _isStopped) == 1)
            { _onError(error); }
        }

        public void OnCompleted()
        {
            if (Interlocked.Increment(ref _isStopped) == 1)
            { _onCompleted(); }
        }
    }
}